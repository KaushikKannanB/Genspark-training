--- 1 Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.
create table summary
(customer_id int primary key,
rentalCount int);

do $$
declare
	rec record;
	cur cursor for (select customer_id, count(*) as Count_rentals from rental group by customer_id);
begin
	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		insert into summary(customer_id, rentalCount) values (rec.customer_id, rec.count_rentals);
	end loop;
end $$;

select * from summary;

--- 2 Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.
create or replace procedure proc_category_count_rent(my_category varchar(100), threshold_count int)
as $$
declare
	rec record;
	cur cursor for (select i.film_id, f.title, count(*) as RCount 
					from rental r join inventory i on r.inventory_id=i.inventory_id 
					join film_category fc on fc.film_id=i.film_id 
					join category c on c.category_id=fc.category_id
					join film f on f.film_id=i.film_id
					where c.name=my_category
					group by i.film_id, f.title
					having count(*)>=threshold_count);
BEGIN
	begin
		open cur;
		loop
			fetch cur into rec;
			exit when not found;
			raise notice 'title: %, Count:%', rec.title, rec.Rcount;
		end loop;
	close cur;
end;
end $$ language plpgsql;

call proc_category_count_rent('Comedy',10)

---3 Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.

create table report(
store_Id int primary key,
dictinct_film_count int
);

do $$
	declare rec record;
begin
	for rec in select store_id, count(*) as scount from inventory group by store_id
	loop
		insert into report(store_Id, dictinct_film_count) values (rec.store_id, rec.scount);
	end loop;
end $$;

select * from report;

---4 Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.
do $$
declare
	rec record;
begin
	for rec in (select customer_id, max(rental_date)as last_rental
				from rental
				group by customer_id
				order by max(rental_date) desc)
	loop
		raise notice 'ID: %, Last rented on: %',rec.customer_id, rec.last_rental;
	end loop;
end $$;


-- Transaction
select * from payment
---1 
select * from customer where email='jane.doe@example.com';
DO $$
DECLARE
    new_customer_id INT;
	new_rental_id int;
BEGIN
    INSERT INTO customer (store_id, first_name, last_name, email, address_id, active, create_date)
    VALUES (1, 'Jane', 'Doe', 'jane.doe@example.com', 5, 1, NOW())
    RETURNING customer_id INTO new_customer_id;

    
    INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
    VALUES (NOW(), 101, new_customer_id, 1);
	returning rental_id into new_rental_id;

	insert into payment(customer_id, staff_id, rental_id, amount, payment_date)
	values(new_customer_id, 1, new_rental_id, 2.00,now());
END;
$$;

--- 2 Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.
do $$
begin
	update rental set rental_date = current_date + Interval '7 days' where customer_id = 601;
 
 
	if exists (select 1 from rental where customer_id = 603) then
		update rental set staff_id = 2 where customer_id = 603;
	else
		raise notice 'Update fails';
		raise exception 'No data found - forcing rollback';
		return;
	end if;
 
	commit;
	exception when others then
		raise notice 'Transaction rolled back - error!';
end;
$$;

--- 3 Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.

BEGIN;

--p1
UPDATE payment SET amount = amount + 10 WHERE payment_id = 17503;


SAVEPOINT sp_before_payment2;

--p2
UPDATE payment SET amount = 0 WHERE payment_id = 2; -- Causes error as payment_id=2 does not exists

ROLLBACK TO SAVEPOINT sp_before_payment2;

--p3
UPDATE payment SET amount = amount + 5 WHERE payment_id = 17504;

COMMIT;

select * from payment order by payment_id;

---4 

select * from rental where inventory_id=2
select * from payment order by payment_date desc

create or replace procedure proc_change_store_id(my_inv_id int)
as $$
DECLARE 
    our_store_id INT;
	our_film_id int;
Begin
	BEGIN
	    SELECT store_id, film_id INTO our_store_id, our_film_id
	    FROM inventory WHERE inventory_id = my_inv_id;
	
	    
	    DELETE FROM payment
	    WHERE rental_id IN (
	        SELECT rental_id FROM rental WHERE inventory_id = my_inv_id
	    );
	
	    
	    DELETE FROM rental
	    WHERE inventory_id = my_inv_id;
	    
	
	    DELETE FROM inventory WHERE inventory_id = my_inv_id;
	
	    IF our_store_id = 1 THEN
	        INSERT INTO inventory(film_id, store_id, last_update)
	        VALUES (our_film_id, 2, NOW());
	    ELSIF our_store_id = 2 THEN
	        INSERT INTO inventory(film_id, store_id, last_update)
	        VALUES (our_film_id, 1, NOW());
	    END IF;
	end;
END $$ language plpgsql;

select * from inventory order by last_update desc
call proc_change_store_id(2);

--- 5
DO $$
BEGIN
    DELETE FROM payment
    WHERE rental_id IN (
        SELECT rental_id FROM rental WHERE inventory_id = 1
    );

    
    DELETE FROM rental
    WHERE inventory_id = 1;
    

    DELETE FROM inventory WHERE inventory_id = 1;
END $$;

--- Triggers:

--1
create or replace function fn_prevent_payment()
returns trigger
as $$
begin
	if new.amount=0 then
	raise exception 'WTH, No payment!!!';
	return new;
	end if;
end $$ language plpgsql;

create or replace trigger trg_zero_pay
before insert on payment
for each row
execute function fn_prevent_payment();

select * from payment;

insert into payment(customer_id, staff_id, rental_id, amount, payment_date)
values(341,2,1520,0,'2025-05-09 14:49:45.738')

---2 

create or replace function fn_update_film()
returns trigger
as $$
begin
	new.last_update:= current_timestamp;
	return new;
end $$ language plpgsql;

create or replace trigger trg_update_film
before update on film
for each row
execute function fn_update_film()

---3 Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.

create table rental_log (
    rental_id int,
    inventory_id int,
    last_update timestamp default current_Timestamp
);
 
select * from rental;
 
create or replace function fn_insert_log()
returns trigger
as $$
declare 
	filmid int;
	rental_count int;
begin
	select i.film_id into filmid from inventory i join film f on i.inventory_id = new.inventory_id;
 
	select count(*) into rental_count from rental r join inventory i on r.inventory_id = i.inventory_id 
		where i.film_id = filmid and r.rental_date >= current_Date - interval '7 days';
 
	if rental_count > 3 then
			insert into rental_log (rental_id, inventory_id) values(new.rental_id,new.inventory_id);
	end if;
	return new;
end;
$$
language plpgsql;
 
 
create trigger tr_log_insert
after insert on rental
for each row 
execute function fn_insert_log();