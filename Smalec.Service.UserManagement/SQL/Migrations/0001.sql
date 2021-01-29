USE [master]
GO

IF DB_ID('UserManagement') IS NULL
    CREATE DATABASE UserManagement
GO

USE [UserManagement]
GO

IF (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Users') IS NULL
BEGIN
    CREATE TABLE [dbo].Users
    (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Login nvarchar(max) NOT NULL,
        Password nvarchar(max) NOT NULL,
        Name nvarchar(max) NOT NULL,
        ProfilePhoto nvarchar(max)
    );
END
GO