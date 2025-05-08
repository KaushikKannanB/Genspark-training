use pubs;
--- use both in and out parameter

select * from products;
create procedure proc_get_products(@pram nvarchar(20),@pcount int out) --- as default all parameters are in, if out mention it.
as
begin
	set @pcount = (select COUNT(*)from products where ---set out parameter and insert value into it
	TRY_CAST(JSON_VALUE(details, '$.spec.ram') as nvarchar(20)) = @pram)
end

declare @count int
exec proc_get_products '8gb', @count out
print @count


--- bulk insert using csv
create table peopledata(Id int primary key, name nvarchar(100), age int)---table for inserting

create table bulkinsertLog(
Id int Identity(1,1) primary key, 
filepath nvarchar(max), 
message nvarchar(max), 
Insertedon DateTime default GetDate())
---table for logging the status of insert

create procedure proc_bulk_insert_csv(@filepath nvarchar(max))
as
begin
	declare @query nvarchar(max) --- declare the inserty query
	set @query = 'BULK INSERT peopledata
	FROM '''+@filepath+'''
	WITH(
	FIRSTROW=2,
	FIELDTERMINATOR='','',
	ROWTERMINATOR=''\n'')'  ---syntax for bulk insert using csv: BULK INSERT tablename FROM filepath with(...)

	exec sp_executesql @query ---execute the insert query inside the sp
end
 --- only for bulk insert without having status logs

--- now lets alter the sp with status logs also
create or alter procedure proc_bulk_insert_csv(@filepath nvarchar(max))
as
begin
---use try catch exception handling
	begin try --try  block
		declare @query nvarchar(max)
		set @query = 'BULK INSERT peopledata
		FROM '''+@filepath+'''
		WITH(
		FIRSTROW=2,
		FIELDTERMINATOR='','',
		ROWTERMINATOR=''\n'')'

		exec sp_executesql @query 
		---after successfully inserting
		---insert log into logstable
		insert into bulkinsertLog(filepath,message) values(
		@filepath, 'Successful bulk insert')
	end try
	begin catch --- catch block if error hits
		insert into bulkinsertLog(filepath,message) values(
		@filepath, ERROR_MESSAGE()) --error message
	end catch
end

exec proc_bulk_insert_csv 'C:\Users\admin\Desktop\Genspark-training\day4-08th-May\daa.csv' -- no such file exists

truncate table peopledata;
exec proc_bulk_insert_csv 'C:\Users\admin\Desktop\Genspark-training\day4-08th-May\data.csv' --- will perform successful insert

select * from peopledata;
select * from bulkinsertLog;
