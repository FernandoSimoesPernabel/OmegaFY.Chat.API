CREATE OR ALTER PROCEDURE [dbo].[sp_ResetDatabaseTables]
AS
BEGIN
    SET NOCOUNT ON;

    -- Chat tables
    DELETE FROM [chat].[MemberMessages]
    DELETE FROM [chat].[Messages]
    DELETE FROM [chat].[Members]
    DELETE FROM [chat].[GroupConfigs]
    DELETE FROM [chat].[Conversations]
    DELETE FROM [chat].[Friendships]
    DELETE FROM [chat].[Users]

    -- Identity tables
    DELETE FROM [dbo].[AspNetUserTokens]
    DELETE FROM [dbo].[AspNetUserRoles]
    DELETE FROM [dbo].[AspNetUserLogins]
    DELETE FROM [dbo].[AspNetUserClaims]
    DELETE FROM [dbo].[AspNetRoleClaims]
    DELETE FROM [dbo].[AspNetUsers]
    DELETE FROM [dbo].[AspNetRoles]
END;