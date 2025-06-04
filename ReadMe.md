Part of my "CLI/Console to Windows" series.

This is a more advanced version using a login window and the ability to add more users. Includes bCrypt for salting and hashing of passwords.
It is a bit rough around the edges for the time being (notably the comments), but we'll get there. ☕

Of course this application won't work unless you have access to a copy of the DB used. Enclosed you will find TestDB.sql which you can use in SSMS to re-create the DB.

You will also need to add a 'user' table to the DB using the enclosed CreateUserTable.sql
