USE [UserManagement]
GO

/* 
add uuid column to prevent our entities from scrapping
leave the numeric Id as a primary key to better performance
*/

IF COL_LENGTH('dbo.Users', 'Uuid') IS NULL
BEGIN
	ALTER TABLE [dbo].Users
	ADD Uuid varchar(40);
END;
GO

IF (SELECT 1 FROM sys.indexes WHERE name='ix_users') IS NULL
BEGIN
    CREATE INDEX ix_users
    ON [dbo].Users(Id);
END
GO

IF (SELECT 1 FROM sys.indexes WHERE name='ix_users_uuid_non_clustered') IS NULL
BEGIN
    CREATE NONCLUSTERED INDEX ix_users_uuid_non_clustered
    ON [dbo].Users(Uuid);
END
GO