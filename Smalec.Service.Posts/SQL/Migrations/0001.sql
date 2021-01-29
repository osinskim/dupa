USE [master]
GO

IF DB_ID('PostsDB') IS NULL
    CREATE DATABASE PostsDB
GO

USE [PostsDB]
GO

IF (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Posts') IS NULL
BEGIN
    CREATE TABLE [dbo].Posts
    (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Uuid varchar(40) NOT NULL,
        UserUuid varchar(40) NOT NULL,
        CreatedDate datetime2 NOT NULL,
        Description nvarchar(max),
        MediaURL nvarchar(max)
    );
END
GO

IF (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Comments') IS NULL
BEGIN
    CREATE TABLE [dbo].Comments
    (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Uuid varchar(40) NOT NULL,
        UserUuid varchar(40) NOT NULL,
        PostUuid varchar(40) NOT NULL,
        PostId int,
        CreatedDate datetime2 NOT NULL,
        Text nvarchar(max) NOT NULL
    );
END
GO

IF (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Reactions') IS NULL
BEGIN
    CREATE TABLE [dbo].Reactions
    (
        Id int IDENTITY(1,1) PRIMARY KEY,
        UserUuid varchar(40) NOT NULL,
        PostUuid varchar(40) NOT NULL,
        CommentUuid varchar(40) NOT NULL,
        CreatedDate datetime2 NOT NULL,
        Type nvarchar(max) NOT NULL,
        CommentId int,
        PostId int
    );
END
GO

IF (SELECT 1 FROM sys.indexes WHERE name='ix_posts') IS NULL
BEGIN
    CREATE INDEX ix_posts
    ON [dbo].Posts(Id);
END
GO

IF (SELECT 1 FROM sys.indexes WHERE name='ix_posts_uuid_non_clustered') IS NULL
BEGIN
    CREATE NONCLUSTERED INDEX ix_posts_uuid_non_clustered
    ON [dbo].Posts(Uuid);
END
GO

IF (SELECT 1 FROM sys.indexes WHERE name='ix_comments') IS NULL
BEGIN
    CREATE INDEX ix_comments
    ON [dbo].Comments(Id);
END
GO

IF (SELECT 1 FROM sys.indexes WHERE name='ix_comments_uuid_non_clustered') IS NULL
BEGIN
    CREATE NONCLUSTERED INDEX ix_comments_uuid_non_clustered
    ON [dbo].Comments(Uuid);
END
GO
