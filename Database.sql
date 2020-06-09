create database distributedAssignment
go

use distributedAssignment
go

create schema dist
go

create table [dist].[Users]
(
	ID uniqueidentifier constraint users_pk primary key default newsequentialid(),
	username nvarchar(max) not null,
	pass nvarchar(max) not null
)

Insert into [dist].[Users] (username,pass)
values ('a','pass')

select *
from [dist].Users