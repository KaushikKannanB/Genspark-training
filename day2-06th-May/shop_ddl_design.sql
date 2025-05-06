create database shop;


use shop;

create table country (Id int primary key, country varchar(100) not null);

create table state (Id int primary key, state varchar(100) not null, country_id int, foreign key (country_id) references country(Id));

create table city (Id int primary key, city varchar(100) not null, state_id int, foreign key (state_id) references state(Id));

create table area (zipcode varchar(10) primary key, name varchar(20) not null, city_id int, foreign key (city_id) references city(Id) );

create table address (Id int primary key, doorno varchar(10), firstline varchar(100), zipcode varchar(10), foreign key (zipcode) references area(zipcode));

create table statusmaster(Id int primary key, status varchar(20) not null); --- status: active, deleted
create table supplier(Id int primary key, name varchar(100) not null, email varchar(255) not null, phone varchar(20) not null, addressid int,statusid int,  foreign key (addressid) references address(Id), foreign key (statusid) references statusmaster(Id));

create table product(Id int primary key, name varchar(100) not null, unit_price decimal(8,2) not null,stock int); --- stock will be dynamic

CREATE TABLE product_supplier (
    transaction_id INT PRIMARY KEY,
    product_id INT,
    supplier_id INT,
    date_of_supply DATE,
    quantity INT,
    FOREIGN KEY (product_id) REFERENCES product(id),
    FOREIGN KEY (supplier_id) REFERENCES supplier(id)
);

--- 

CREATE TABLE customer (
    id INT PRIMARY KEY,
    name VARCHAR(100),
    phone VARCHAR(20),
    age INT,
    address_id INT,
    FOREIGN KEY (address_id) REFERENCES address(id)
);

create table orders (Id int primary key, customer_id int, amount decimal(8,2),date_of_order date, order_status int, foreign key (customer_id) references customer(Id));
--- amount will be calculated dynamically

create table order_details (Id int primary key, product_id int, order_number int, unit_price decimal(8,2), quantity int, foreign key(order_number) references orders(Id), foreign key (product_id) references product(Id));
--- unit price x quantity -> amount in orders
create table order_status_master(Id int primary key, statusmessage varchar(20) not null); --- order status message: 

alter table orders add constraint fk foreign key (order_status) references order_status_master(Id);


---date of order constraints, all not null constraints, stored procedures for order amount calculations.


