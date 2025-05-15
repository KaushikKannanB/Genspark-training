-- Encryption, Decryption and Masking

1. Create a stored procedure to encrypt a given text
Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL pgcrypto extension.

first lemme create pgcrypto extension:

create extension pgcrypto;

--- my secret key: 'MyS3cur3P@ssPhrase!'

now after this i will use pgp_sym_encrypt to encrypt my data

create or replace function fn_encrypt_data(in name text, in secret_key text)
returns bytea
as $$
begin
	return pgp_sym_encrypt(name, secret_key);
end $$ language plpgsql;

select fn_encrypt_data('Kaushik','MyS3cur3P@ssPhrase!');

-- to decrypt:
select pgp_sym_decrypt(fn_encrypt_data('Kaushik','MyS3cur3P@ssPhrase!'),'MyS3cur3P@ssPhrase!');

----

2. Create a stored procedure to compare two encrypted texts
Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

create or replace function fn_compare_encrypted_texts(name1 bytea, name2 bytea, secret_key text)
returns boolean
as $$
declare 
	decrypted_name1 text;
	decrypted_name2 text;
begin
	decrypted_name1 := pgp_sym_decrypt(name1, secret_key);
	decrypted_name2 := pgp_sym_decrypt(name2, secret_key);

	if decrypted_name1=decrypted_name2 then
		return true;
	else
		return false;
	end if;
end $$ language plpgsql;

select fn_compare_encrypted_texts(fn_encrypt_data('Kaushik','MyS3cur3P@ssPhrase!'),fn_encrypt_data('Kaushikkannan','MyS3cur3P@ssPhrase!'),'MyS3cur3P@ssPhrase!');

------

 3. Create a stored procedure to partially mask a given text
Task: Write a procedure sp_mask_text that:

Shows only the first 2 and last 2 characters of the input string

Masks the rest with *

create or replace function fn_masking(name text)
returns text
as $$
begin
	return left(name,2) || repeat('*', length(name)-2) || right(name,2);
end $$ language plpgsql;

select fn_masking('Dharun');

----

4. Create a procedure to insert into customer with encrypted email and masked name
Task:

Call sp_encrypt_text for email

Call sp_mask_text for first_name

Insert masked and encrypted values into the customer table

Use any valid address_id and store_id to satisfy FK constraints.

create table customers
(cid serial primary key,
store_id int,
fname text,
lname text,
email bytea,addressid int, activebool boolean, createdate timestamp, last_update timestamp);
select * from customers;

create or replace procedure proc_insert_encrypted_masked_customer(secret_key text, n_store_id int, firstname text, lastname text, n_email text, address_id_n int, activebool_n boolean)
as $$
declare
	encrypted_mail bytea;
	masked_fname text;
	masked_lname text;
begin
	begin
		encrypted_mail := fn_encrypt_data(n_email, secret_key);
		masked_fname := fn_masking(firstname); ---cannot be unmasked,,, so if u need the original sometime, save the original into another table,,,
		masked_lname := fn_masking(lastname); ---cannot be unmasked,,, so if u need the original sometime, save the original into another table,,,

		insert into customers(store_id, fname, lname, email, addressid, activebool, createdate, last_update)
		values (n_store_id, masked_fname, masked_lname, encrypted_mail, address_id_n,activebool_n, current_timestamp, current_timestamp);
	end;
end $$ language plpgsql;

call proc_insert_encrypted_masked_customer('MyS3cur3P@ssPhrase!',1,'Katniss','Everdeen','katnisseverdeen@gmail.com',1, true);

select * from customers;

----

5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
Task:
Write sp_read_customer_masked() that:

Loops through all rows

Decrypts email

Displays customer_id, masked first name, and decrypted email


create or replace procedure proc_iterate_customers_decrypt(secret_key text)
as $$
declare 
	rec record;
	cur cursor for (select * from customers);
	decrypted_mail text;
begin
	open cur;
	loop
		fetch cur into rec;
		exit when not found;
		decrypted_mail := pgp_sym_decrypt(rec.email, secret_key);
		raise notice 'Id: %, Customer name: % %, MailId: %',rec.cid, rec.fname, rec.lname,decrypted_mail;
	end loop;
end $$ language plpgsql;

call proc_iterate_customers_decrypt('MyS3cur3P@ssPhrase!');