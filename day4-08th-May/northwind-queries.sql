
---1
select o.orderid, c.contactname as customername ,concat(e.firstname,' ',e.lastname) as employeename from Orders o join Customers c on o.CustomerID = c.CustomerID join employees e on o.EmployeeID =e.EmployeeID; 

---2

select p.productname, c.categoryname as category, s.companyname as suppliername from Products p join Categories c on p.categoryid = c.categoryid join Suppliers s on p.supplierid = s.supplierid

---3

select o.orderId, p.productname, od.unitprice,od.quantity from Orders o join [Order Details] od on o.OrderId=od.OrderId join Products p on od.ProductId=p.Productid

---4
select * from employees where reportsto is not null;

---5

select customerid, count(*) as No_orders from Orders group by customerid;

---6
select c.categoryid, c.categoryname,avg(p.unitprice) as average from Categories c join Products p on c.categoryid = p.categoryid group by c.categoryid,c.categoryname

---7
select * from customers where contacttitle like 'Owner%'

---8
select top 5 * from Products order by unitprice desc;

---9

select o.orderid, sum(od.unitprice * od.quantity) as totalamount from Orders o join [Order Details] od on o.orderid = od.orderid group by o.orderid

---10
create OR alter procedure proc_show_orders_of_customer(@cid nvarchar(20))
as
begin
	select * from orders where customerid=@cid
end

exec proc_show_orders_of_customer 'VICTE'

---11
create OR alter procedure proc_insert_product( @pname nvarchar(100), @supplierid int, @catid int,@qpu nvarchar(100) )
as
begin
	insert into Products(productname,supplierid, categoryid, quantityperunit, unitprice, unitsinstock, unitsonorder, reorderlevel, discontinued) values ( @pname, @supplierid,@catid,@qpu,9.00,  29, 0,0,0)
end

exec proc_insert_product 'Apple pie',8,3,'2 pieces per box'

---12
create or alter procedure proc_sales_per_employee(@eid int)
as
begin
	select o.employeeid, sum(od.unitprice * od.quantity) as totalamount 
	from Orders o join [Order Details] od on o.orderid = od.orderid 
	group by o.employeeid having o.employeeid=@eid
end

exec proc_sales_per_employee 9

---13
create or alter procedure proc_categorise_price(@cid int)
as
begin
	select productname, categoryid,unitprice, rank() over (partition by categoryid order by unitprice desc)as Rank from Products where categoryid=@cid;
end

exec proc_categorise_price 1

---14
create or alter procedure proc_revenue(@pid int)
as
begin
	with productrevenue as
	(select ProductId, sum(unitprice*quantity) as Amount from [order details] group by productid)

	select * from productrevenue where productid=@pid
end

exec proc_revenue 11

---15
with headmanager as(

select employeeid, concat(titleofcourtesy+' ' +firstname,' ',lastname) as Name from employees where reportsto=2

union all
select e.employeeid, concat(e.titleofcourtesy+' ' +e.firstname,' ',e.lastname) as Name from employees e join headmanager hm on e.reportsto=hm.employeeid
)
select * from headmanager;
