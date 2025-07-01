CREATE DATABASE TestDBMS
use TestDBMS
CREATE TABLE person(
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