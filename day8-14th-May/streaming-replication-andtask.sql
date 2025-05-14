Replication query: 

navigate to postgre-bin
initdb -D "D:/pri"
---creates a db inside the folder "pri"

---enter config file and chane port number from 5432 to 5433
pg_ctl -D D:\pri -o "-p 5433" -l d:\pri\logfile start
--- starts the server

>psql -p 5433 -d postgres -c "CREATE ROLE replicator with REPLICATION LOGIN PASSWORD 'repl_pass';"
--- creates a replicator role for the db making it primary server
pg_basebackup -D d:\sec -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433
--- creates a replica of 5433 in d:\sec folder

--enter config file and change port number into 5435

--open another cmd as administrator
pg_ctl -D D:\sec -o "-p 5435" -l d:\sec\logfile start
--- start the server - secondary


psql -p 5433 -d postgres 
---in primary: connects to the "postgres" db now u can query 

---(In another cmd)
psql -p 5435 -d postgres
---in sec: (standby): connects to the "postgres" db now u can query
--------------------------------------
--- 5433: primary
select * from pg_stat_replication;
5435
select pg_is_in_recovery();
-------------------------------------

---------------------------------------------------------------------------

create table rental_log
(logId serial primary key, 
rental_id timestamp, 
customer_id int,
film_id int, 
amount numeric(8,2), 
log_time timestamp default current_timestamp);


create or replace procedure proc_insert_rental_log(p_customer_id int, p_film_id int, amount numeric(8,2))
postgres-# as $$
postgres$# begin
postgres$# begin
postgres$# insert into rental_log(rental_id, customer_id, film_id, amount)values(current_timestamp, p_customer_id, p_film_id, amount);
postgres$# exception when others then
postgres$# raise notice 'Error:%',sqlerrm;
postgres$# end;
postgres$# end $$ language plpgsql;

create or replace function fn_trg_update_log()
postgres-# returns trigger
postgres-# as $$
postgres$# begin
postgres$# insert into rental_update_log(log_id, oldamount, newamount) values (new.logId, old.amount, new.amount);
postgres$# return new;
postgres$# end $$ language plpgsql;

create or replace trigger trg_upd_log
postgres-# after update on rental_log
postgres-# for each row
postgres-# execute function fn_trg_update_log();

update rental_log set amount=10.99 where logid=1;

--- now from the standby servers we can view updates in the rental log table as well the rental update log table

