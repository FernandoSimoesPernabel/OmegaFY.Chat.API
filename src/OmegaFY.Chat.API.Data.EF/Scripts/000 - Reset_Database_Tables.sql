CREATE OR ALTER PROCEDURE [dbo].[sp_ResetDatabaseTables]
AS
BEGIN
    SET NOCOUNT ON;

    -- Desabilita verificação de FKs temporariamente
    EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

    -- Limpa tabelas na ordem correta (filhos primeiro)
    DELETE FROM [chat].[MemberMessages]
    DELETE FROM [chat].[Messages]
    DELETE FROM [chat].[Members]
    DELETE FROM [chat].[GroupConfigs]
    DELETE FROM [chat].[Conversations]
    DELETE FROM [chat].[Friendships]
    DELETE FROM [chat].[Users]

    -- Limpa tabelas do Identity
    DELETE FROM [dbo].[AspNetUserTokens]
    DELETE FROM [dbo].[AspNetUserRoles]
    DELETE FROM [dbo].[AspNetUserLogins]
    DELETE FROM [dbo].[AspNetUserClaims]
    DELETE FROM [dbo].[AspNetRoleClaims]
    DELETE FROM [dbo].[AspNetUsers]
    DELETE FROM [dbo].[AspNetRoles]

    -- Reabilita verificação de FKs
    EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'
END