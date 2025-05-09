---1 Write a cursor that loops through all films and prints titles longer than 120 minutes.
do $$
declare records record;
begin
	for records in select * from film where length>120
	loop
		raise notice 'Title:%, Duration:%', records.title, records.length;
	end loop;
end $$


do $$
declare records record; cur cursor for select * from film;
begin
	open cur;
	loop
		fetch cur into records;
		exit when not found;
		if records.length>120 then
			raise notice 'Title: %, Duration: %', records.title, records.length;
		end if;
	end loop;
end $$

---2 Create a cursor that iterates through all customers and counts how many rentals each made.

do $$
declare rec record; cur cursor for (select customer_id, count(*) as Count_rentals from rental group by customer_id);
begin
	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		raise notice 'Customer:%, Count:%', rec.customer_id, rec.Count_rentals;
	end loop;
end $$

---3 Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.
select * from film where film_id=874

do $$
declare r record;
begin
	for r in (select f.film_id
			from film f left outer join inventory i on f.film_id = i.film_id
			left outer join rental re on i.inventory_id=re.inventory_id
			group by f.film_id
			having count(re.rental_id)<5)
	loop
		update film
		set rental_rate = rental_rate+1 where film_id = r.film_id;
	end loop;
end $$

---4 Create a function using a cursor that collects titles of all films from a particular category.
select * from film
create or replace function fn_all_films_category(categ varchar(100))
returns table(films varchar(100))
as $$
declare rec record;
begin
	for rec in select film.title as Titles from film_category f join category c on f.category_id=c.category_id join film on film.film_id = f.film_id
	loop
		films:= rec.Titles;
		return next;
	end loop;
end $$ language plpgsql;

select * from fn_all_films_category('Action')

---5 Loop through all stores and count how many distinct films are available in each store using a cursor.

create or replace function fn_distinct_films_in_store(st int)
returns int
as $$ 
declare rec record;
begin
	for rec in select store_id, count(*) as scount from inventory group by store_id
	loop
		if rec.store_id=st then return rec.scount;
		end if;
	end loop;
	
end $$ language plpgsql;

select fn_distinct_films_in_store(1)
