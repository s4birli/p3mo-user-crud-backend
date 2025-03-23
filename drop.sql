-- drop.sql - Script to clean up the database completely
-- WARNING: This will delete ALL data and remove ALL tables!

USE [db-mehmet];
GO

PRINT 'Disabling all constraints...';
-- Disable all constraints to avoid dependency issues
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

PRINT 'Deleting all data from tables...';
-- Delete all data from tables
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UserDetails')
    DELETE FROM [UserDetails];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
    DELETE FROM [Users];

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
    DELETE FROM [Roles];

IF EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
    DELETE FROM [__EFMigrationsHistory];

PRINT 'Dropping foreign key constraints...';
-- Drop all foreign key constraints first
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.foreign_keys;

EXEC sp_executesql @sql;

PRINT 'Dropping indexes...';
-- Drop all indexes except primary keys (those will be dropped with the tables)
SET @sql = N'';

SELECT @sql += N'
DROP INDEX ' + QUOTENAME(i.name) + ' ON ' + QUOTENAME(OBJECT_SCHEMA_NAME(i.object_id)) + '.' + QUOTENAME(OBJECT_NAME(i.object_id)) + ';'
FROM sys.indexes i
JOIN sys.tables t ON i.object_id = t.object_id
WHERE i.is_primary_key = 0 -- exclude primary keys
  AND i.is_unique_constraint = 0 -- exclude unique constraints 
  AND i.type_desc <> 'HEAP' -- exclude heaps
  AND OBJECT_NAME(i.object_id) NOT LIKE 'sys%'; -- exclude system tables

IF LEN(@sql) > 0
    EXEC sp_executesql @sql;

PRINT 'Dropping tables...';
-- Drop tables in proper order
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UserDetails')
BEGIN
    DROP TABLE [UserDetails];
    PRINT 'Dropped table UserDetails';
END

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    DROP TABLE [Users];
    PRINT 'Dropped table Users';
END

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    DROP TABLE [Roles];
    PRINT 'Dropped table Roles';
END

IF EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    DROP TABLE [__EFMigrationsHistory];
    PRINT 'Dropped table __EFMigrationsHistory';
END

PRINT 'Database cleanup completed successfully.';
GO 