-- * Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)

CREATE TABLE students (
    student_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(15) UNIQUE NOT NULL
);

CREATE TABLE courses (
    course_id SERIAL PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL,
    category VARCHAR(50),
    duration_days INT CHECK (duration_days > 0)
);

CREATE TABLE trainers (
    trainer_id SERIAL PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    expertise VARCHAR(100) NOT NULL
);


CREATE TABLE enrollments (
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    enroll_date DATE NOT NULL DEFAULT CURRENT_DATE,

	foreign key (student_id) references students(student_id),
	foreign key (course_id) references courses(course_id),
	unique(student_id, course_id)

);


CREATE TABLE certificates (
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT UNIQUE NOT NULL,  -- one cert per enrollment
    issue_date DATE NOT NULL DEFAULT CURRENT_DATE,
    serial_no VARCHAR(50) UNIQUE NOT NULL,

    FOREIGN KEY (enrollment_id) REFERENCES enrollments(enrollment_id)
);

CREATE TABLE course_trainers (
    course_id INT NOT NULL,
    trainer_id INT NOT NULL,

    PRIMARY KEY (course_id, trainer_id),
    FOREIGN KEY (course_id) REFERENCES courses(course_id),
    FOREIGN KEY (trainer_id) REFERENCES trainers(trainer_id)
);

----

-- * Insert sample data using `INSERT` statements

INSERT INTO students (name, email, phone) VALUES
('Alice Smith', 'alice@example.com', '1111111111'),
('Bob Johnson', 'bob@example.com', '2222222222'),
('Charlie Lee', 'charlie@example.com', '3333333333'),
('Diana Patel', 'diana@example.com', '4444444444'),
('Ethan Brown', 'ethan@example.com', '5555555555'),
('Fiona Davis', 'fiona@example.com', '6666666666'),
('George Miller', 'george@example.com', '7777777777');

INSERT INTO courses (course_name, category, duration_days) VALUES
('Python Basics', 'Programming', 30),
('Advanced Java', 'Programming', 45),
('Data Science 101', 'Data Science', 60),
('Web Development', 'Frontend', 40),
('Database Design', 'Backend', 35),
('AI Fundamentals', 'AI/ML', 50);

INSERT INTO trainers (trainer_name, expertise) VALUES
('Dr. John Doe', 'Machine Learning'),
('Jane Roe', 'Python Development'),
('Mark Spencer', 'Web Development'),
('Linda Nguyen', 'Databases'),
('Tom Hudson', 'Java Expert');

INSERT INTO course_trainers (course_id, trainer_id) VALUES
(1, 2),  -- Python Basics → Jane Roe
(2, 5),  -- Advanced Java → Tom Hudson
(3, 1),  -- Data Science → John Doe
(4, 3),  -- Web Dev → Mark Spencer
(5, 4),  -- DB Design → Linda Nguyen
(6, 1);  -- AI → John Doe

INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES
(1, 1, '2025-01-15'),  -- Alice → Python
(2, 2, '2025-02-01'),  -- Bob → Java
(3, 3, '2025-02-10'),  -- Charlie → DS
(4, 4, '2025-03-01'),  -- Diana → Web
(5, 5, '2025-03-15'),  -- Ethan → DB
(6, 6, '2025-04-01'),  -- Fiona → AI
(1, 3, '2025-04-10'),  -- Alice → DS
(2, 4, '2025-04-15');  -- Bob → Web

INSERT INTO certificates (enrollment_id, issue_date, serial_no) VALUES
(1, '2025-02-15', 'CERT2025-001'),  -- Alice for Python
(2, '2025-03-01', 'CERT2025-002'),  -- Bob for Java
(3, '2025-03-15', 'CERT2025-003'),  -- Charlie for DS
(4, '2025-04-10', 'CERT2025-004'),  -- Diana for Web
(5, '2025-04-25', 'CERT2025-005'),  -- Ethan for DB
(6, '2025-05-10', 'CERT2025-006');  -- Fiona for AI

----

-- * Create indexes on `student_id`, `email`, and `course_id`

-- syntax: create index index_name on table_name(column_name);
create index idx_student on students(student_id);
create index idx_email on students(email);
create index idx_course on courses(course_id);

---

-- Phase 3: SQL Joins Practice

-- Write queries to:

-- 1. List students and the courses they enrolled in

select s.name, c.course_name 
from enrollments e join courses c on e.course_id=c.course_id 
join students s on e.student_id=s.student_id;

-- 2. Show students who received certificates with trainer names


select s.name, c.course_name, t.trainer_name, ce.serial_no
from certificates ce join enrollments e on ce.enrollment_id=e.enrollment_id
join students s on e.student_id=s.student_id
join courses c on e.course_id = c.course_id
join course_trainers ct on ct.course_id=e.course_id
join trainers t on t.trainer_id=ct.trainer_id;

-- 3. Count number of students per course

select c.course_name, count(*)
from enrollments e join courses c on e.course_id=c.course_id
group by c.course_name
order by count(*)

----

-- Phase 4: Functions & Stored Procedures

-- Function:

-- Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates.

create or replace function fn_get_certified_students(c_id INT)
returns table(studentid varchar(100))
as $$
declare rec record;
begin
	for rec in (select s.name as List from certificates ce join enrollments e on ce.enrollment_id=e.enrollment_id join students s on e.student_id=s.student_id where e.course_id=c_id)
	loop
		studentid:=rec.List; --- append 
		return next;
	end loop;
end $$ language plpgsql;

select * from fn_get_certified_students(1);

----

-- Stored Procedure:

-- Create `sp_enroll_student(p_student_id, p_course_id)`
-- → Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).

create or replace procedure proc_enroll_student(p_stid int, p_cid int, flag boolean)
as $$
declare
	new_enrollment_id int;
begin
	insert into enrollments(student_id, course_id, enroll_date) values(p_stid, p_cid, now())
	returning enrollment_id into new_enrollment_id;

	if flag then
		insert into certificates(enrollment_id, issue_date, serial_no) values (new_enrollment_id, now(),'CERT2025-007');
	end if;
end $$ language plpgsql;

call proc_enroll_student(1,5,true);

---

-- Phase 5: Cursor

-- Use a cursor to:

-- * Loop through all students in a course
-- * Print name and email of those who do not yet have certificates

do $$
declare
	rec record;
begin
	for rec in (select s.name as name ,s.email as email, e.course_id as cid from enrollments e left join certificates ce on e.enrollment_id=ce.enrollment_id join students s on e.student_id=s.student_id where ce.certificate_id is null)
	loop
		raise notice 'Name: %, Email: %, CourseId: %', rec.name, rec.email, rec.cid;
	end loop;
end $$;

----

-- Phase 6: Security & Roles

-- 1. Create a `readonly_user` role:

--    * Can run `SELECT` on `students`, `courses`, and `certificates`
--    * Cannot `INSERT`, `UPDATE`, or `DELETE`
   

-- 2. Create a `data_entry_user` role:

--    * Can `INSERT` into `students`, `enrollments`
--    * Cannot modify certificates directly


create role read_only_user login password 'helloread';

grant select on students, certificates , courses to read_only_user;
--- select: students, certificates and courses for readonlyuser;

create role data_entry_user login password 'hellowrite';

grant insert on students, enrollments to data_entry_user;
--- 
set role read_only_user; --- switching to read_only_user

select * from enrollments; --- permission denied
select * from students;

reset role; --- switch back to postgres
---
set role data_entry_user; --- switching to data entry user
select * from students; --- permission denied
reset role;
---
select current_user; --- see the current user

--list all the users
select rolname
from pg_roles
where rolcanlogin=true

---
-- Phase 7: Transactions & Atomicity

-- Write a transaction block that:

-- * Enrolls a student
-- * Issues a certificate
-- * Fails if certificate generation fails (rollback)

create or replace procedure proc_transaction(pstid int, pcid int, flag boolean)
as $$
declare
	new_enrollment_id int;
begin
	begin
		insert into enrollments(student_id, course_id, enroll_date) values(pstid, pcid, now())
		returning enrollment_id into new_enrollment_id;

		if flag then
			insert into certificates(enrollment_id, issue_date, serial_no) values (new_enrollment_id, now(),'CERT2025-008');
		end if;

	exception when others then
		--- implict rollback
		raise notice 'Error: %', sqlerrm;
	end;
end $$ language plpgsql;

call proc_transaction(2,5,true);

---

