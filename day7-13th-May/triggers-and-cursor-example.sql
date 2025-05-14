-- Triggers: create a function that returns trigger and its function, and then create a trigger that calls the function 

create table update_accounts(
Id serial primary key,
oldname varchar(100),
newname varchar(100)
);


create or replace function fn_trigger_tbl_account_update()
returns trigger
as $$
declare
	colname text := tg_argv[0];
	tablename text := tg_argv[1];
	o_value text;
	n_value text;
begin
	execute format('select ($1).%I::text',colname) into o_value using old;
	execute format('select ($1).%I::text',colname) into n_value using new;

	if n_value is distinct from o_value
	then
		insert into update_accounts(oldname, newname) values (o_value, n_value);
	end if;

	return new;
end $$ language plpgsql;

create trigger trg_tbl_account_update
before update on tbl_accounts
for each row
execute function fn_trigger_tbl_account_update('name','name');

update tbl_accounts set name='Chadwick' where name='Chad';

select * from update_accounts;


cursors: declare a cursor, record... open loop, fetch each record,

-- use 1: print each row in a notice,exception
do $$
declare
	rec record;
	cur cursor for (select * from tbl_accounts);
begin
	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		if rec.balance>4000
		then
			raise notice 'Name:%, Balance:%', rec.name, rec.balance;
		end if;
	end loop;
end $$;


--- use 2:cursor to insert into another table:

create table tbl_accounts_taxes
(Id int primary key,
name varchar(100),
tax numeric(8,2));

do $$
declare
	rec record;
	cur cursor for (select * from tbl_accounts);
begin
	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		insert into tbl_accounts_taxes(Id, name, tax) values
		(rec.id, rec.name, rec.balance*0.01);
	end loop;
	
end $$;

select * from tbl_accounts_taxes