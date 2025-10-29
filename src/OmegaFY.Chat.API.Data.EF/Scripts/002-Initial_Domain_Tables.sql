CREATE TABLE chat.Users (
    Id uniqueidentifier NOT NULL,
    Email varchar(320) NOT NULL,
    DisplayName varchar(100) NOT NULL,

    CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT UQ_Users_Email UNIQUE (Email)
);

CREATE TABLE chat.Friendships (
	Id UNIQUEIDENTIFIER NOT NULL,
	RequestingUserId UNIQUEIDENTIFIER NOT NULL,
	InvitedUserId UNIQUEIDENTIFIER NOT NULL,
	StartedDate DATETIME2 NOT NULL,
	[Status] VARCHAR(10) NOT NULL,

	CONSTRAINT PK_Friendships PRIMARY KEY (Id, RequestingUserId, InvitedUserId),
	CONSTRAINT FK_Users_Friendships_RequestingUserId FOREIGN KEY (RequestingUserId) REFERENCES chat.Users (Id),
	CONSTRAINT FK_Users_Friendships_InvitedUserId FOREIGN KEY (InvitedUserId) REFERENCES chat.Users (Id)
);

GO

CREATE TABLE chat.Conversations (
	Id UNIQUEIDENTIFIER NOT NULL,
	[Type] VARCHAR(15) NOT NULL,
	[Status] VARCHAR(10) NOT NULL,
	CreatedDate DATETIME2 NOT NULL

	CONSTRAINT PK_Conversations PRIMARY KEY (Id)
);

CREATE TABLE chat.GroupConfigs (
	Id UNIQUEIDENTIFIER NOT NULL,
	ConversationId UNIQUEIDENTIFIER NOT NULL,
	CreatedByUserId UNIQUEIDENTIFIER NOT NULL,
	GroupName NVARCHAR(100) NOT NULL,
	MaxNumberOfMembers TINYINT NOT NULL,

	CONSTRAINT PK_GroupConfigs PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_GroupConfigs_ConversationId FOREIGN KEY (ConversationId) REFERENCES chat.Conversations (Id),
	CONSTRAINT FK_Users_GroupConfigs_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES chat.Users (Id)
);

CREATE TABLE chat.[Members] (
	Id UNIQUEIDENTIFIER NOT NULL,
	ConversationId UNIQUEIDENTIFIER NOT NULL,
	UserId UNIQUEIDENTIFIER NOT NULL,
	JoinedDate DATETIME2 NOT NULL,

	CONSTRAINT PK_Members PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_Members_ConversationId FOREIGN KEY (ConversationId) REFERENCES chat.Conversations (Id),
	CONSTRAINT FK_Users_Members_UserId FOREIGN KEY (UserId) REFERENCES chat.Users (Id),
	CONSTRAINT UQ_Members_ConversationId_UserId UNIQUE (ConversationId, UserId)
)

CREATE TABLE chat.[Messages] (
	Id UNIQUEIDENTIFIER NOT NULL,
	ConversationId UNIQUEIDENTIFIER NOT NULL,
	SenderMemberId UNIQUEIDENTIFIER NOT NULL,
	JoinedDate DATETIME2 NOT NULL,
	[Type] VARCHAR(20) NOT NULL,
	Content NVARCHAR(1000) NOT NULL,

	CONSTRAINT PK_Messages PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_Messages_ConversationId FOREIGN KEY (ConversationId) REFERENCES chat.Conversations (Id),
	CONSTRAINT FK_Users_Messages_SenderMemberId FOREIGN KEY (SenderMemberId) REFERENCES chat.[Members] (Id)
)

CREATE TABLE chat.MemberMessages (
	Id UNIQUEIDENTIFIER NOT NULL,
	ConversationId UNIQUEIDENTIFIER NOT NULL,
	SenderMemberId UNIQUEIDENTIFIER NOT NULL,
	DestinationMemberId UNIQUEIDENTIFIER NOT NULL,
	DeliveryDate DATETIME2 NOT NULL,
	[Status] VARCHAR(20) NOT NULL,

	CONSTRAINT PK_MemberMessages PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_MemberMessages_ConversationId FOREIGN KEY (ConversationId) REFERENCES chat.Conversations (Id),
	CONSTRAINT FK_Users_MemberMessages_SenderMemberId FOREIGN KEY (SenderMemberId) REFERENCES chat.[Members] (Id),
	CONSTRAINT FK_Users_MemberMessages_DestinationMemberId FOREIGN KEY (DestinationMemberId) REFERENCES chat.[Members] (Id)
)

GO