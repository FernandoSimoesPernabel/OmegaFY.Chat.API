-- SQLite Domain Tables

CREATE TABLE Users (
	Id TEXT NOT NULL,
	Email TEXT NOT NULL,
	DisplayName TEXT NOT NULL,

	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT UQ_Users_Email UNIQUE (Email)
);

CREATE TABLE Friendships (
	Id TEXT NOT NULL,
	RequestingUserId TEXT NOT NULL,
	InvitedUserId TEXT NOT NULL,
	StartedDate TEXT NOT NULL,
	Status TEXT NOT NULL,

	CONSTRAINT PK_Friendships PRIMARY KEY (Id, RequestingUserId, InvitedUserId),
	CONSTRAINT FK_Users_Friendships_RequestingUserId FOREIGN KEY (RequestingUserId) REFERENCES Users (Id),
	CONSTRAINT FK_Users_Friendships_InvitedUserId FOREIGN KEY (InvitedUserId) REFERENCES Users (Id)
);

CREATE TABLE Conversations (
	Id TEXT NOT NULL,
	Type TEXT NOT NULL,
	Status TEXT NOT NULL,
	CreatedDate TEXT NOT NULL,

	CONSTRAINT PK_Conversations PRIMARY KEY (Id)
);

CREATE TABLE GroupConfigs (
	Id TEXT NOT NULL,
	ConversationId TEXT NOT NULL,
	CreatedByUserId TEXT NOT NULL,
	GroupName TEXT NOT NULL,
	MaxNumberOfMembers INTEGER NOT NULL,

	CONSTRAINT PK_GroupConfigs PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_GroupConfigs_ConversationId FOREIGN KEY (ConversationId) REFERENCES Conversations (Id),
	CONSTRAINT FK_Users_GroupConfigs_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES Users (Id)
);

CREATE TABLE Members (
	Id TEXT NOT NULL,
	ConversationId TEXT NOT NULL,
	UserId TEXT NOT NULL,
	JoinedDate TEXT NOT NULL,

	CONSTRAINT PK_Members PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_Members_ConversationId FOREIGN KEY (ConversationId) REFERENCES Conversations (Id),
	CONSTRAINT FK_Users_Members_UserId FOREIGN KEY (UserId) REFERENCES Users (Id),
	CONSTRAINT UQ_Members_ConversationId_UserId UNIQUE (ConversationId, UserId)
);

CREATE TABLE Messages (
	Id TEXT NOT NULL,
	ConversationId TEXT NOT NULL,
	SenderMemberId TEXT NOT NULL,
	SendDate TEXT NOT NULL,
	Type TEXT NOT NULL,
	Content TEXT NOT NULL,

	CONSTRAINT PK_Messages PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_Messages_ConversationId FOREIGN KEY (ConversationId) REFERENCES Conversations (Id),
	CONSTRAINT FK_Users_Messages_SenderMemberId FOREIGN KEY (SenderMemberId) REFERENCES Members (Id)
);

CREATE TABLE MemberMessages (
	Id TEXT NOT NULL,
	MessageId TEXT NOT NULL,
	SenderMemberId TEXT NOT NULL,
	DestinationMemberId TEXT NOT NULL,
	DeliveryDate TEXT NOT NULL,
	Status TEXT NOT NULL,

	CONSTRAINT PK_MemberMessages PRIMARY KEY (Id),
	CONSTRAINT FK_Conversations_MemberMessages_MessageId FOREIGN KEY (MessageId) REFERENCES Messages (Id),
	CONSTRAINT FK_Users_MemberMessages_SenderMemberId FOREIGN KEY (SenderMemberId) REFERENCES Members (Id),
	CONSTRAINT FK_Users_MemberMessages_DestinationMemberId FOREIGN KEY (DestinationMemberId) REFERENCES Members (Id)
);