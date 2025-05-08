---functions
--functions has to return a value


---scalar value function: return a scalar value, just a value... we can use it in a select query

--syntax: create function fname(in parameters) returns out type as begin return ..... end
create or alter function fn_tax_calculation(@base int, @tax int)
returns int
as
begin
	return (@base+(@base*@tax/100))
end


select dbo.fn_tax_calculation(1000,10)


---use it in a table
select title,price, dbo.fn_tax_calculation(price, 10) as taxed from titles;

---table value function: return a table value, does not need begin and end
create or alter function fn_sampletable(@minprice float)
returns table
as
 return select title, price from titles where price>@minprice

select * from dbo.fn_sampletable(20.00)


--older way of creating an empty table inside the function and populating it... this requires begin and end

create or alter function fn_sampletableOld(@minprice float)
returns @result table(bookname nvarchar(100),price float)
as
begin
	insert into @result select title,price from titles where price>=@minprice;
	return
end
go
select * from dbo.fn_sampletableOld(17.00)