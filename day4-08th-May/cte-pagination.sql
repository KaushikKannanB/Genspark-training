--- CTE's common table expressions
--- whatever we write within cte will be stored as object which can be usedn in the further queries

--- example:
with cteauthors ---syntax: with ctename as sql query
as
(select au_id, au_fname from authors)


select * from cteauthors

--pagination using cte
declare @page int =2, @pagesize int = 10;
with cteforpaging as
	(select title, price, ROW_NUMBER() over (order by price desc)as rownum from titles)

	select * from cteforpaging where rownum between ((@page - 1) * @pagesize + 1) AND (@page * @pagesize)

--pagination using cte in a sp
create or alter procedure proc_paginated_books(@page int, @pagesize int)
as
begin
	with cteforpaging as
	(select title, price, ROW_NUMBER() over (order by price desc)as rownum from titles)

	select * from cteforpaging where rownum between ((@page - 1) * @pagesize + 1) AND (@page * @pagesize)
end

exec proc_paginated_books 1,10

--instead we can use offset also for pagination
select title,price
from titles
order by price desc
offset 0 rows fetch next 10 rows only ---offset no_of_rows_to_leave rows fetch next numberofrows rows only

