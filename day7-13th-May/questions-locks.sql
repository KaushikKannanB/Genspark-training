---1 Try two concurrent updates to same row → see lock in action.
select * from tbl_accounts;
--A
begin;
update tbl_accounts set balance=balance+100 where name='Chad';
--B
begin;
update tbl_accounts set balance=balance-100 where name='Chad'; -- waits for A to commit or rollback

---2 Write a query using SELECT...FOR UPDATE and check how it locks row.
begin;
select * from tbl_accounts where name='Chad' for update;

begin;
update tbl_accounts set balance=balance+200 where name='Chad'; -- waits for the update in A to finish

---3 Intentionally create a deadlock and observe PostgreSQL cancel one transaction.
-- A
begin;
update tbl_accounts set balance=5000 where name='Chad';

update tbl_accounts set balance=5000 where name='Alice';

-- B
begin;
update tbl_accounts set balance=5000 where name='Alice';

update tbl_accounts set balance=5000 where name='Chad';

-- ERROR:  deadlock detected
-- Process 15140 waits for ShareLock on transaction 1093; blocked by process 22692.
-- Process 22692 waits for ShareLock on transaction 1094; blocked by process 15140. 

-- SQL state: 40P01
-- Detail: Process 15140 waits for ShareLock on transaction 1093; blocked by process 22692.
-- Process 22692 waits for ShareLock on transaction 1094; blocked by process 15140.
-- Hint: See server log for query details.
-- Context: while updating tuple (0,51) in relation "tbl_accounts"

---4 Use pg_locks query to monitor active locks.
