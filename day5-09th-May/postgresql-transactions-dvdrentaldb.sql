---11 Write a transaction that inserts a customer and an initial rental in one atomic operation.
DO $$
DECLARE
    new_customer_id INT;
BEGIN
    INSERT INTO customer (store_id, first_name, last_name, email, address_id, active, create_date)
    VALUES (1, 'Jane', 'Doe', 'jane.doe@example.com', 5, 1, NOW())
    RETURNING customer_id INTO new_customer_id;

    
    INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
    VALUES (NOW(), 101, new_customer_id, 1);
END;
$$;

select * from inventory where film_id=8


select * from inventory
---12 Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.
do $$
declare n_film_id int;
begin
	update film
	set title = 'Good Bad Ugly' where film_id=8
	returning film_id into n_film_id;

	insert into inventory(film_id, store_id, last_update)
	values (n_film_id, 1, current_timestamp);

	raise exception 'stop updating!';
end $$

---13 Create a transaction that transfers an inventory item from one store to another.
select * from inventory where inventory_id=1;

do $$
declare our_store_id int;
begin
	select store_id into our_store_id
	from inventory where inventory_id=1;
	
	if our_store_id =1
		then update inventory set store_id =2 where inventory_id=1;
	elseif our_store_id =2
		then update inventory set store_id =1 where inventory_id=1;
	end if;

	raise notice 'store changed';
end $$;

---14 Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.

BEGIN;

-- Update 1
UPDATE payment
SET amount = amount + 1
WHERE payment_id = 1;

-- Create SAVEPOINT
SAVEPOINT before_second;

-- Update 2 (we'll undo this)
UPDATE payment
SET amount = amount + 5
WHERE payment_id = 2;

-- Rollback only the second update
ROLLBACK TO SAVEPOINT before_second;

-- Update 3
UPDATE payment
SET amount = amount + 10
WHERE payment_id = 3;

-- Commit the transaction
COMMIT;


---15 Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

BEGIN;
-- Replace with actual customer_id
DO $$
DECLARE
    target_customer_id INTEGER := 524;  -- Change as needed
BEGIN
    -- First, delete payments made by the customer
    DELETE FROM payment
    WHERE customer_id = target_customer_id;

    -- Then, delete rentals made by the customer
    DELETE FROM rental
    WHERE customer_id = target_customer_id;

    -- Finally, delete the customer
    DELETE FROM customer
    WHERE customer_id = target_customer_id;

   
    RAISE NOTICE 'Customer %, rentals, and payments deleted.', target_customer_id;

END $$;

COMMIT;
