use pubs;
go


select * from titleauthor
select * from titles
select * from authors

select publishers.pub_id, pub_name from titles right join publishers on titles.pub_id=publishers.pub_id where title is null;

select pub_id, pub_name from publishers where pub_id not in (select distinct pub_id from titles);

select title, concat(authors.au_fname ,' ',  authors.au_lname) as name from titles join titleauthor on titles.title_id = titleauthor.title_id join authors on titleauthor.au_id = authors.au_id order by 2;

select * from titles
select * from publishers
select * from sales
select * from stores

select titles.title_id,titles.title, publishers.pub_name, ord_date from titles join publishers on titles.pub_id = publishers.pub_id join sales on titles.title_id = sales.title_id order by 1;


select p.pub_name, min(s.ord_date) as FirstOrder from titles t right join publishers p on t.pub_id = p.pub_id left join sales s on t.title_id = s.title_id group by p.pub_name order by 2;

select title, concat(st.stor_address,', ',st.city, ', ', st.state, ', ', st.zip) as address from titles t left join sales s on t.title_id = s.title_id left join stores st on s.stor_id = st.stor_id order by 2 desc;


---stored procedures::

create procedure proc_firstprocedure as
begin
	print 'Hey Presidio!'
end

exec proc_firstprocedure

create table products(
	Id int identity(1,1) primary key,
	name  nvarchar(100)not null,
	details nvarchar(max) not null
	);

go

create procedure insert_products(@productname nvarchar(100), @productdetails nvarchar(max) ---in paramters:@variablename type)
as
begin
	insert into products(name,details)values (@productname,@productdetails)
end

go

insert_products 'Laptop', '{"brand": "Dell", "spec": {"ram":"16gb"}}'; ---json

select * from products;

select id, JSON_QUERY(details, '$.spec') from products; --- json query for printing whole json

create procedure update_ram(@pid int,@newvalueram nvarchar(20))
as
begin
	update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalueram) where Id=@pid ---modify(column, what value in json to modify, new value)
end

create procedure update_brand (@pid int,@newbrand nvarchar(100))
as
begin
	update products set details = JSON_MODIFY(details,'$.brand',@newbrand) where Id = @pid
end

update_brand 1,'HP'
update_ram 1,'8gb'

select * from products;


--- need single specific value from json
select name, JSON_VALUE(details, '$.brand') as Brand from products; --- json  value(column, what value in json do u need)

--- bulk insert from json

---  create table for inserting:

create table posts(
	userId int,
	Id int primary key, 
	title nvarchar(100), 
	body nvarchar(max)
);

---create storedprocedure for extrating values from jsondata and inserting it in

create procedure bulk_insert_posts(@jsondata nvarchar(max))
as
begin
	insert into posts(userId, Id,title, body)
	select userId,Id,title,body from openjson(@jsondata) with (userId int, id int,title nvarchar(100), body nvarchar(max));
	---extract json using openjson(json) and parse the json and provide schema for it,,, and select from it and pass to insert
end

--- declare sample json
declare @jsondata nvarchar(max)= '
	[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  },
  {
    "userId": 1,
    "id": 3,
    "title": "ea molestias quasi exercitationem repellat qui ipsa sit aut",
    "body": "et iusto sed quo iure\nvoluptatem occaecati omnis eligendi aut ad\nvoluptatem doloribus vel accusantium quis pariatur\nmolestiae porro eius odio et labore et velit aut"
  }]
'
--after declaring run it along declaring
exec bulk_insert_posts @jsondata

select * from posts;
--- bulk inserted

--- create procedure to filter posts using userid
create procedure take_posts_using_userid(@uid int)
as
begin
	select * from posts where userId=@uid
end

exec take_posts_using_userid 1