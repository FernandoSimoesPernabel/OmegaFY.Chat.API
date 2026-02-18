-- Reset Database Script (SQLite)

-- Chat tables
DELETE FROM MemberMessages;
DELETE FROM Messages;
DELETE FROM Members;
DELETE FROM GroupConfigs;
DELETE FROM Conversations;
DELETE FROM Friendships;
DELETE FROM Users;

-- Identity tables
DELETE FROM AspNetUserTokens;
DELETE FROM AspNetUserRoles;
DELETE FROM AspNetUserLogins;
DELETE FROM AspNetUserClaims;
DELETE FROM AspNetRoleClaims;
DELETE FROM AspNetUsers;
DELETE FROM AspNetRoles;