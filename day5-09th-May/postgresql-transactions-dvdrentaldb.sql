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


---12 Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.
DO $$
BEGIN
    -- Step 1: Update a film
    UPDATE film
    SET title = 'Exploding Kittens: The Movie'
    WHERE film_id = 8;

    -- Step 2: Insert into inventory
    INSERT INTO inventory (film_id, store_id, last_update)
    VALUES (8, 1, CURRENT_TIMESTAMP);

    -- Step 3: Simulate failure
    RAISE EXCEPTION 'Simulated failure: Rolling back entire transaction';

    -- COMMIT is implicit in DO block if no errors occur
END;
$$;

---13 Create a transaction that transfers an inventory item from one store to another.
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM inventory WHERE inventory_id = 100 AND store_id = 1
    ) THEN
        UPDATE inventory
        SET store_id = 2,
            last_update = CURRENT_TIMESTAMP
        WHERE inventory_id = 100;
    ELSE
        RAISE NOTICE 'Inventory not found in the specified source store.';
    END IF;
END;
$$;

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

    -- You can raise a notice to confirm before commit
    RAISE NOTICE 'Customer %, rentals, and payments deleted.', target_customer_id;

END $$;

COMMIT;
