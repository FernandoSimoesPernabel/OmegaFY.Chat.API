CREATE SCHEMA [chat];
GO

CREATE TABLE [chat].[Users] 
(
    [Id] uniqueidentifier NOT NULL,
    [Email] varchar(320) NOT NULL UNIQUE,
    [DisplayName] varchar(100) NOT NULL,

    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
	CONSTRAINT UQ_Email UNIQUE (Email)
);

GO