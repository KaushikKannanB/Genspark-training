use pubs;

select title from titles; 

select title from titles where pub_id=1389;

select title, price from titles where price between 10 and 15;

select title, price from titles where price is null;

select title from titles where title like 'The%';

select title from titles where title not like '%v%';

select title, royalty from titles order by royalty;

select title, price,type, pub_id from titles order by price desc, type, pub_id desc;

select avg(price), type from titles group by type;

select distinct type from titles;

select top 2 title, price from titles order by price desc;

select title, price, type, advance from titles where type='business' and price<20 and advance>7000;

select pub_id, count(*) from titles where price between 15 and 20 and title like '%it%' group by pub_id having count(*)>2;

select au_fname, state from authors where state='CA';

select state, count(*) from authors group by state;

