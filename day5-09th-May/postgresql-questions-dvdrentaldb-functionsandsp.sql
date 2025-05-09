select title, length, rental_rate  from film order by length desc;

select r.customer_id as Name, count(*) as RentalCount
from customer c join rental r on c.customer_id=r.customer_id
group by r.customer_id
order by RentalCount desc
limit 5;

select f.film_id, count(r.rental_id) as rentedcount
from film f left outer join inventory i on f.film_id = i.film_id
left outer join rental r on i.inventory_id=r.inventory_id
group by f.film_id
having count(r.rental_id)=0
order by f.film_id

select first_name, last_name from actor where actor_id in(
select actor_id from film_actor where film_id =(
select film_id from film where title='Academy Dinosaur'))

select customer_id, sum(amount) as TotalAmount, count(*) as RentCount,
from payment
group by customer_id
order by sum(amount);

with ctemovies as
(select f.film_id, count(*) as RentedCount
from film f join inventory i on f.film_id=i.film_id
join rental r on i.inventory_id=r.inventory_id
group by f.film_id
order by RentedCount desc)

select * from ctemovies limit 3

with cte_count as
(select customer_id, count(*) as count_rents from rental group by customer_id)
,cte_avg as
(select avg(count_rents) as avg_rents
from cte_count
)
select customer_id from cte_count where count_rents>=(select * from cte_avg)


create or replace function fn_rentalCount_cid(cid int)
returns int as $$
declare count_rent int;
begin
	select count(*) into count_rent
	from rental where customer_id=cid;

	return count_rent;
end;
$$ language plpgsql

select fn_rentalCount_cid(459)

create or replace procedure proc_update_rental(fid int, newrental float)
as $$
begin
	update film
	set rental_rate = newrental where film_id=fid;
end;
$$ language plpgsql


call proc_update_rental(384, 5.99)

select * from film where film_id=384

create or replace procedure proc_overdue()
as $$
declare records record;
begin
	for records in
		select * from rental where return_date is null and rental_date<current_date - interval '7 days'
	loop
		raise notice 'rentalid: %, customer:%', records.rental_id, records.customer_id;
	end loop;
end;
$$ language plpgsql


call proc_overdue()
