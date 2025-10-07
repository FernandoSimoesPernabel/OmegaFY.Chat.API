CREATE SCHEMA chat;
GO

CREATE TABLE chat.Users
(
    Id uniqueidentifier NOT NULL,
    Email varchar(320) NOT NULL UNIQUE,
    DisplayName varchar(100) NOT NULL,

    CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT UQ_Email UNIQUE (Email)
);

CREATE TABLE chat.Friendships (
	Id UNIQUEIDENTIFIER NOT NULL,
	RequestingUserId UNIQUEIDENTIFIER NOT NULL,
	InvitedUserId UNIQUEIDENTIFIER NOT NULL,
	StartedDate DATETIME2 NOT NULL,
	[Status] VARCHAR(10) NOT NULL,

	CONSTRAINT PK_Friendships PRIMARY KEY (Id, RequestingUserId, InvitedUserId),
	CONSTRAINT FK_Users_Requesting FOREIGN KEY (RequestingUserId) REFERENCES chat.Users (Id),
	CONSTRAINT FK_Users_Invited FOREIGN KEY (InvitedUserId) REFERENCES chat.Users (Id)
);

GO