CREATE DATABASE TestDBMS
use TestDBMS
CREATE TABLE my_schema.users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL
)

use TestDBMS
CREATE TABLE my_schema.person(
	person_id int IDENTITY(1,1) PRIMARY KEY,
	first_name varchar(255) NOT NULL,
	last_name varchar(255) NOT NULL,
	phone varchar(25) NULL,
	email varchar(255) NOT NULL,
	street varchar(255) NULL,
	city varchar(50) NULL,
	country varchar(25) NULL,
	zip_code varchar(5) NULL
)

-- Add superuser 'Wazi' with pw 'test' if missing
insert into my_schema.users (username,password_hash) 
values ('Wazi','$2a$12$1btVx9WXHNswIemGLXx5uuNDlrJiR5ZMw.teOWgvI6Xu4Q0Ah39A6')

-- Test data
insert into my_schema.person (first_name, last_name, email) 
values 
('John','Hanson','test1'),
('Bruce','Wayne','email'),
('Alex','Murphy','ED-209')