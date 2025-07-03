CREATE DATABASE "TestDB"
CREATE TABLE my_schema.users (
    user_id INTEGER PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    username TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
)

CREATE TABLE my_schema.person
(
    person_ID INTEGER PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    first_name text COLLATE pg_catalog."default" NOT NULL,
    last_name text COLLATE pg_catalog."default" NOT NULL,
    phone text COLLATE pg_catalog."default",
    email text COLLATE pg_catalog."default" NOT NULL,
    street text COLLATE pg_catalog."default",
    zip_code text COLLATE pg_catalog."default",
    city text COLLATE pg_catalog."default",
    country text COLLATE pg_catalog."default"    
)

-- Add superuser 'Wazi' with pw 'test' if missing
insert into my_schema.users (username,password_hash) 
values ('Wazi','$2a$12$1btVx9WXHNswIemGLXx5uuNDlrJiR5ZMw.teOWgvI6Xu4Q0Ah39A6')

-- Test data
insert into my_schema.person (first_name, last_name, email) 
values 
('John','Hanson','test1'),
('Bruce','Wayne','email')
('Alex','Murphy','ED-209')