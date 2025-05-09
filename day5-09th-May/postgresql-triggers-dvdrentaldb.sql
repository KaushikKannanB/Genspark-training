
---6 Write a trigger that logs whenever a new customer is inserted.

select * from customer;

create table customer_log
(customer_id int primary key,
time_inserted timestamp default current_timestamp)

create or replace function fn_trigger_customer_log()
returns trigger
as $$
begin
	insert into customer_log(customer_id) values (new.customer_id);
	return new;
end $$ language plpgsql;

create trigger trg_add_customer
after insert on customer
for each row
execute function fn_trigger_customer_log();
	
select * from customer_log
select * from customer


insert into customer(store_id, first_name, last_name, email, address_id, activebool, create_date, last_update, active)
values (1,'Martha','Stewart','mail',345,true,'2025-05-09', '2025-05-09 14:49:45.738',1 )

---7 Create a trigger that prevents inserting a payment of amount 0.
create or replace function fn_prevent_payment()
returns trigger
as $$
begin
	if new.amount=0 then
	raise exception 'WTH, No payment!!!';
	return new;
	end if;
end $$ language plpgsql;

create trigger trg_zero_pay
before insert on payment
for each row
execute function fn_prevent_payment();

select * from payment;

insert into payment(customer_id, staff_id, rental_id, amount, payment_date)
values(341,2,1520,0,'2025-05-09 14:49:45.738')

---8 Set up a trigger to automatically set last_update on the film table before update.

select * from film

create or replace function fn_update_film()
returns trigger
as $$
begin
	new.last_update:= current_timestamp;
	return new;
end $$ language plpgsql;

create trigger trg_update_film
before update on film
for each row
execute function fn_update_film()

update film set title='Junglebook' where film_id=8
select * from film where film_id=8


---9 Create a trigger to log changes in the inventory table (insert/delete).

select * from inventory;

create table inventory_log(
	film_id int,
	action_message varchar(100)
);

create or replace function fn_trg_ins_del_inventory()
returns trigger
as $$
begin
	if tg_op='INSERT' then
		insert into inventory_log(film_id, action_message)
		values(new.film_id, 'inserted');
	elseif tg_op='DELETE' then
		insert into inventory_log(film_id, action_message)
		values (old.film_id, 'deleted');
	end if;
	return new;
end $$ language plpgsql;

create trigger trg_insdel_inventory
after insert or delete on inventory
for each row
execute function fn_trg_ins_del_inventory()

select * from inventory_log

insert into inventory(film_id, store_id, last_update)
values (8,1, '2025-05-09 14:49:45.738')

delete from inventory where film_id=8 and store_id=1

---10 Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.

create or replace function fn_prevenr_rentals()
returns trigger
as $$
begin
	if new.customer_id in (select customer_id from payment group by customer_id having sum(amount)>50.00)
	then raise exception 'No rentals allowed for you man!';
	end if;
	return new;
end $$ language plpgsql;

create trigger trg_prevent_rental
before insert on rental
for each row
execute function fn_prevenr_rentals()

insert into rental(rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
values('2025-05-09 14:49:45.738', 3419, 184, '2025-05-09 14:49:45.738',1,'2025-05-09 14:49:45.738')

