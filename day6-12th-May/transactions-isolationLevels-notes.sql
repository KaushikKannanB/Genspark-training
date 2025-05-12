create table tbl_accounts(
Id serial primary key,
name varchar(20),
balance float
)

insert into tbl_accounts(name, balance) values
('Alice', 5000)

insert into tbl_accounts(name, balance) values
('Bob', 3000)

select * from tbl_accounts

-- basic transaction

begin;

update tbl_accounts
set balance = balance-500
where name='Alice';

update tbl_accounts
set balance=balance+500
where name='Bob';

commit;

-- all commands are defaultly autocommitted... but when we put a set of commands
-- inside begin and commit... it is taken as a single unit and committed
-- altogether

select * from tbl_accounts;

-- transaction with an error
begin;

update tbl_accounts
set balance = balance-500
where name='Alice';

update tbl_accounts 
set balance=balance+500
where name='Bob';

commit;

-- after getting an error, you need to rollback so u can go back to the previous stable state
rollback;


-- rollback with savepoint

begin;

update tbl_accounts
set balance = balance-500
where name='Alice';

savepoint updated_alice;

update tbl_account -- error strikes here, so I am gonna rollback to my savepoint
set balance=balance+500
where name='Bob';

commit;

rollback to updated_alice;
select * from tbl_accounts; -- 500 deducted from alice....

update tbl_accounts
set balance=balance+500
where name='Bob';

select * from tbl_accounts; --- we added 500 now to bob tooo

---raising notice
--- note: u cannot use commit/rollback inside a do command as if the block inside
-- do executes successfully - autocommitted, if not automatically rollbacks

-- do runs an implicit transaction ***
do $$
declare current_balance float;
begin
	select balance into current_balance
	from tbl_accounts where name = 'Alice';

	if current_balance>4500 
	then
		update tbl_accounts set balance=balance-4500 where name='Alice';
		
		update tbl_accounts set balance=balance+4500 where name='Bob';
	--- do block autocomitts here
	else
		raise notice 'Insufficient fund on Alice';
		--- automatic rollbacks
	end if;
end $$;

select * from tbl_accounts;

---best practices for transaction: try and catch for rollbacks

do $$
begin
	begin
		update tbl_accounts
		set balance = balance-100
		where name='Alice';
		
		update tbl_accounts
		set balance=balance+100
		where name='Bob';
		--- autcommits
	exception
		when others then
			raise notice 'rollbacked';

			---rollbacks
	end;
end $$;

select * from tbl_accounts;
-- if u need to rollback to a certain savepoint... remove it from do block as exception handling and savepoint
-- rollback cannot coexist in a do block as it does not read or process explicit commit and rollbacks

-- or go inside a funtion



--- concurrency control:

---*** MVCC: Multi versio concurrency control: allows multiple versions of the same data to exist temporarily,
--- so readers dont block writers and vice versa

-- *** isolation levels: 4: 1.read uncommitted -> not supported in postgres;
						-- 2. read committed -> default; 3. repeatable read -> ensures repeatable reads
							-- 4. serialisable -> full isolation(safe but slow, no dirty reads, no lost update)

--- read committed:
-- You select a row. Someone updates it and commits. If you select again in the same transaction, you’ll see the new value. uses mvcc(in the same hood)

--repeatable read:
-- You select a row. Even if someone updates and commits, your second select will still show the old value — same snapshot.


-- problems appear without concurrency control:
---***1::dirty read: reading uncommitted data which might disappear:

-- example: transaction a - updates a row doesnt commit
-- transaction b -  reads the updated row
-- transaction c - rolls back

-- so tb has read a row that didnt exist officially

-- step1: create a transaction A

begin;
update tbl_accounts set balance=5000 where name='Alice';

---uncommitted

---step2: create transaction b
set transaction isolation level read uncommitted;
begin;
select * from tbl_accounts where name='Alice';

---step3: rollback
rollback

select * from tbl_accounts;


---***2:: lost update:
-- transaction a reads a record, transaction b reads a record, b updates and commits, a holds the old value and overwrites b's changes

---lost update

-- solutions for lost update:
-- 1. pessimistic locking: explicit locking
-- 2. optimistic locking: 
-- 3. serialisable isolation level

--- Inconsisten read/read Anomalies
-- 1. Dirty read:
-- 2. Non repeatable read: A non-repeatable read happens when a transaction reads the same row twice and gets different values each time, 
-- because another transaction modified and committed the row in between the two reads.

-- transaction a reads a record, transaction b updates and COMMIT the record, transaction a reads the record and see a different
-- transaction A
begin;
select * from tbl_accounts;
-- transaction B
begin;
update tbl_accounts set balance=4500 where name='Alice';
commit;
-- transaction A
begin;
select * from tbl_accounts;


-- 3. Phantom read
rollback
-- transaction a: 
begin;
select * from tbl_accounts where balance>4000; --- returns Alice's record

---transaction b: --adds a data
begin;
insert into tbl_accounts(name, balance) values ('Chad', 4100);
commit;

---transaction a:
begin;
select * from tbl_accounts where balance>4000; --- returns Alice's and Chad's record



