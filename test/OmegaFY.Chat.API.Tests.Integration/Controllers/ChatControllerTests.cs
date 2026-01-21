using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;
using OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;
using OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;
using OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;
using OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;
using OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversations;
using OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Tests.Integration.Base;
using OmegaFY.Chat.API.Tests.Integration.Constants;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.Tests.Integration.Controllers;

public class ChatControllerTests : IntegrationTestBase
{
    public ChatControllerTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task CreateGroupConversation_WithValidData_ReturnsCreated()
    {
        // Arrange
        string email = $"create-group-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Create Group User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object request = new { GroupName = "Test Group", MaxNumberOfMembers = (byte)10 };

        // Act
        HttpResponseMessage response = await PostAsync("/api/chat", request, token);
        ApiResponse<CreateGroupConversationCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotEqual(Guid.Empty, content.Data.ConversationId);
    }

    [Fact]
    public async Task CreateGroupConversation_WithEmptyGroupName_ReturnsBadRequest()
    {
        // Arrange
        string email = $"empty-groupname-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Empty Group Name User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object request = new { GroupName = "", MaxNumberOfMembers = (byte)10 };

        // Act
        HttpResponseMessage response = await PostAsync("/api/chat", request, token);
        ApiResponse<CreateGroupConversationCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task CreateGroupConversation_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        object request = new { GroupName = "Test Group", MaxNumberOfMembers = (byte)10 };

        // Act
        HttpResponseMessage response = await PostAsync("/api/chat", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetConversationById_WithValidConversationId_ReturnsConversation()
    {
        // Arrange
        string email = $"get-conv-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Get Conversation User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Get Conversation Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{createContent.Data.ConversationId}", token);
        ApiResponse<GetConversationByIdQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetConversationByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Conversation);
    }

    [Fact]
    public async Task GetConversationById_WithNonExistentConversationId_ReturnsNotFound()
    {
        // Arrange
        string email = $"nonexistent-conv-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Nonexistent Conversation User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentConversationId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{nonExistentConversationId}", token);
        ApiResponse<GetConversationByIdQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetConversationByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task GetConversationById_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{conversationId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUserConversations_WithValidToken_ReturnsOk()
    {
        // Arrange
        string email = $"user-conversations-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "User Conversations User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync("/api/chat/me/conversations", token);
        ApiResponse<GetUserConversationsQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserConversationsQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.UserConversations);
    }

    [Fact]
    public async Task GetUserConversations_WithExistingConversation_ReturnsConversationsInList()
    {
        // Arrange
        string email = $"user-conv-list-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "User Conv List User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "List Test Group", MaxNumberOfMembers = (byte)10 };
        await PostAsync("/api/chat", createRequest, token);

        // Act
        HttpResponseMessage response = await GetAsync("/api/chat/me/conversations", token);
        ApiResponse<GetUserConversationsQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserConversationsQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotNull(content.Data);
        Assert.NotEmpty(content.Data.UserConversations);
    }

    [Fact]
    public async Task GetUserConversations_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        HttpResponseMessage response = await GetAsync("/api/chat/me/conversations");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUserUnreadMessages_WithValidToken_ReturnsOk()
    {
        // Arrange
        string email = $"unread-messages-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Unread Messages User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync("/api/chat/me/get-unread-messages", token);
        ApiResponse<GetUserUnreadMessagesQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserUnreadMessagesQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Messages);
    }

    [Fact]
    public async Task GetUserUnreadMessages_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        HttpResponseMessage response = await GetAsync("/api/chat/me/get-unread-messages");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SendMessage_WithValidData_ReturnsCreated()
    {
        // Arrange
        string email = $"send-msg-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Send Message User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Send Message Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object messageRequest = new { Type = MessageType.Normal, Body = "Hello, World!" };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages", messageRequest, token);
        ApiResponse<SendMessageCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendMessageCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotEqual(Guid.Empty, content.Data.MessageId);
        Assert.Equal(createContent.Data.ConversationId, content.Data.ConversationId);
    }

    [Fact]
    public async Task SendMessage_WithEmptyBody_ReturnsBadRequest()
    {
        // Arrange
        string email = $"empty-body-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Empty Body User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Empty Body Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object messageRequest = new { Type = MessageType.Normal, Body = "" };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages", messageRequest, token);
        ApiResponse<SendMessageCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendMessageCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task SendMessage_ToNonExistentConversation_ReturnsNotFound()
    {
        // Arrange
        string email = $"msg-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Msg Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentConversationId = Guid.NewGuid();

        object messageRequest = new { Type = MessageType.Normal, Body = "Hello!" };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{nonExistentConversationId}/messages", messageRequest, token);
        ApiResponse<SendMessageCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendMessageCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task SendMessage_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        object messageRequest = new { Type = MessageType.Normal, Body = "Hello!" };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{conversationId}/messages", messageRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUserConversationMessages_WithValidConversationId_ReturnsOk()
    {
        // Arrange
        string email = $"conv-messages-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Conv Messages User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Conv Messages Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/me/{createContent.Data.ConversationId}/messages", token);
        ApiResponse<GetUserConversationMessagesQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserConversationMessagesQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Messages);
    }

    [Fact]
    public async Task GetUserConversationMessages_WithNonExistentConversationId_ReturnsOkWithEmptyMessages()
    {
        // Arrange
        string email = $"conv-msg-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Conv Msg Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentConversationId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/me/{nonExistentConversationId}/messages", token);
        ApiResponse<GetUserConversationMessagesQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserConversationMessagesQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotNull(content.Data);
        Assert.Empty(content.Data.Messages);
    }

    [Fact]
    public async Task GetUserConversationMessages_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/me/{conversationId}/messages");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMessageFromMember_WithValidMessageId_ReturnsMessage()
    {
        // Arrange
        string creatorEmail = $"get-message-creator-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"get-message-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Get Message Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Get Message Member", TestConstants.DEFAULT_PASSWORD);

        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);
        string memberToken = await AuthenticateAsync(memberEmail, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Get Message Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, creatorToken);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = memberUser.UserId };
        await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, creatorToken);

        object messageRequest = new { Type = MessageType.Normal, Body = "Test Message" };
        HttpResponseMessage sendResponse = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages", messageRequest, creatorToken);
        ApiResponse<SendMessageCommandResult> sendContent = await sendResponse.Content.ReadFromJsonAsync<ApiResponse<SendMessageCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Wait for async event processing to create MemberMessage records
        await Task.Delay(5000);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{createContent.Data.ConversationId}/messages/{sendContent.Data.MessageId}", memberToken);
        ApiResponse<GetMessageFromMemberQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetMessageFromMemberQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Message);
    }

    [Fact]
    public async Task GetMessageFromMember_WithNonExistentMessageId_ReturnsNotFound()
    {
        // Arrange
        string email = $"msg-nonexistent-get-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Msg Nonexistent Get User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Msg Nonexistent Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        Guid nonExistentMessageId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{createContent.Data.ConversationId}/messages/{nonExistentMessageId}", token);
        ApiResponse<GetMessageFromMemberQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetMessageFromMemberQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task GetMessageFromMember_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        Guid messageId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{conversationId}/messages/{messageId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task MarkMessageAsRead_WithValidMessageId_ReturnsNoContent()
    {
        // Arrange
        string creatorEmail = $"mark-read-creator-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"mark-read-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Mark Read Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Mark Read Member", TestConstants.DEFAULT_PASSWORD);

        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);
        string memberToken = await AuthenticateAsync(memberEmail, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Mark Read Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, creatorToken);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = memberUser.UserId };
        await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, creatorToken);

        object messageRequest = new { Type = MessageType.Normal, Body = "Message to be read" };
        HttpResponseMessage sendResponse = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages", messageRequest, creatorToken);
        ApiResponse<SendMessageCommandResult> sendContent = await sendResponse.Content.ReadFromJsonAsync<ApiResponse<SendMessageCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Wait for async event processing to create MemberMessage records
        await Task.Delay(10000);

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages/{sendContent.Data.MessageId}/mark-as-read", new { }, memberToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task MarkMessageAsRead_WithNonExistentMessageId_ReturnsNotFound()
    {
        // Arrange
        string email = $"mark-read-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Mark Read Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Mark Read Nonexistent Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        Guid nonExistentMessageId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages/{nonExistentMessageId}/mark-as-read", new { }, token);
        ApiResponse<MarkMessageAsReadCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<MarkMessageAsReadCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task MarkMessageAsRead_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        Guid messageId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{conversationId}/messages/{messageId}/mark-as-read", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddMemberToGroup_WithValidUserId_ReturnsCreated()
    {
        // Arrange
        string creatorEmail = $"add-member-creator-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"add-member-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Add Member Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Add Member Member", TestConstants.DEFAULT_PASSWORD);
        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Add Member Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, creatorToken);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = memberUser.UserId };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, creatorToken);
        ApiResponse<AddMemberToGroupCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<AddMemberToGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotEqual(Guid.Empty, content.Data.MemberId);
        Assert.Equal(createContent.Data.ConversationId, content.Data.ConversationId);
    }

    [Fact]
    public async Task AddMemberToGroup_WithNonExistentUserId_ReturnsInternalServerError()
    {
        // Arrange
        string email = $"add-member-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Add Member Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Add Member Nonexistent Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = Guid.NewGuid() };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, token);
        ApiResponse<AddMemberToGroupCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<AddMemberToGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task AddMemberToGroup_WithEmptyGuid_ReturnsBadRequest()
    {
        // Arrange
        string email = $"add-member-empty-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Add Member Empty User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Add Member Empty Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = Guid.Empty };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, token);
        ApiResponse<AddMemberToGroupCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<AddMemberToGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task AddMemberToGroup_ToNonExistentConversation_ReturnsNotFound()
    {
        // Arrange
        string creatorEmail = $"add-member-no-conv-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"add-member-no-conv-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Add Member No Conv Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Add Member No Conv Member", TestConstants.DEFAULT_PASSWORD);
        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);

        Guid nonExistentConversationId = Guid.NewGuid();
        object addMemberRequest = new { UserId = memberUser.UserId };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{nonExistentConversationId}/members", addMemberRequest, creatorToken);
        ApiResponse<AddMemberToGroupCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<AddMemberToGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task AddMemberToGroup_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        object addMemberRequest = new { UserId = Guid.NewGuid() };

        // Act
        HttpResponseMessage response = await PostAsync($"/api/chat/{conversationId}/members", addMemberRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMemberFromConversation_WithValidMemberId_ReturnsMember()
    {
        // Arrange
        string creatorEmail = $"get-member-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"get-member-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Get Member Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Get Member Member", TestConstants.DEFAULT_PASSWORD);
        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Get Member Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, creatorToken);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = memberUser.UserId };
        HttpResponseMessage addResponse = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, creatorToken);
        ApiResponse<AddMemberToGroupCommandResult> addContent = await addResponse.Content.ReadFromJsonAsync<ApiResponse<AddMemberToGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{createContent.Data.ConversationId}/members/{addContent.Data.MemberId}", creatorToken);
        ApiResponse<GetMemberFromConversationQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetMemberFromConversationQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Member);
    }

    [Fact]
    public async Task GetMemberFromConversation_WithNonExistentMemberId_ReturnsNotFound()
    {
        // Arrange
        string email = $"get-member-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Get Member Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Get Member Nonexistent Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        Guid nonExistentMemberId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{createContent.Data.ConversationId}/members/{nonExistentMemberId}", token);
        ApiResponse<GetMemberFromConversationQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetMemberFromConversationQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task GetMemberFromConversation_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        Guid memberId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/chat/{conversationId}/members/{memberId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ChangeGroupConfig_WithValidData_ReturnsOk()
    {
        // Arrange
        string email = $"change-config-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Change Config User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Original Group Name", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object changeConfigRequest = new { NewGroupName = "Updated Group Name", NewMaxNumberOfMembers = (byte)20 };

        // Act
        HttpResponseMessage response = await PutAsync($"/api/chat/{createContent.Data.ConversationId}/group-config", changeConfigRequest, token);
        ApiResponse<ChangeGroupConfigCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<ChangeGroupConfigCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.Equal("Updated Group Name", content.Data.GroupName);
        Assert.Equal(20, content.Data.MaxNumberOfMembers);
    }

    [Fact]
    public async Task ChangeGroupConfig_WithEmptyGroupName_ReturnsBadRequest()
    {
        // Arrange
        string email = $"change-config-empty-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Change Config Empty User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Original Group Name", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object changeConfigRequest = new { NewGroupName = "", NewMaxNumberOfMembers = (byte)20 };

        // Act
        HttpResponseMessage response = await PutAsync($"/api/chat/{createContent.Data.ConversationId}/group-config", changeConfigRequest, token);
        ApiResponse<ChangeGroupConfigCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<ChangeGroupConfigCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task ChangeGroupConfig_ToNonExistentConversation_ReturnsNotFound()
    {
        // Arrange
        string email = $"change-config-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Change Config Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentConversationId = Guid.NewGuid();

        object changeConfigRequest = new { NewGroupName = "Updated Name", NewMaxNumberOfMembers = (byte)20 };

        // Act
        HttpResponseMessage response = await PutAsync($"/api/chat/{nonExistentConversationId}/group-config", changeConfigRequest, token);
        ApiResponse<ChangeGroupConfigCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<ChangeGroupConfigCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task ChangeGroupConfig_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        object changeConfigRequest = new { NewGroupName = "Updated Name", NewMaxNumberOfMembers = (byte)20 };

        // Act
        HttpResponseMessage response = await PutAsync($"/api/chat/{conversationId}/group-config", changeConfigRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveMemberFromGroup_WithValidMemberId_ReturnsNoContent()
    {
        // Arrange
        string creatorEmail = $"remove-member-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"remove-member-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Remove Member Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Remove Member Member", TestConstants.DEFAULT_PASSWORD);
        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Remove Member Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, creatorToken);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = memberUser.UserId };
        HttpResponseMessage addResponse = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, creatorToken);
        ApiResponse<AddMemberToGroupCommandResult> addContent = await addResponse.Content.ReadFromJsonAsync<ApiResponse<AddMemberToGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/chat/{createContent.Data.ConversationId}/members/{addContent.Data.MemberId}", creatorToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemoveMemberFromGroup_WithNonExistentMemberId_ReturnsInternalServerError()
    {
        // Arrange
        string email = $"remove-member-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Remove Member Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Remove Member Nonexistent Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        Guid nonExistentMemberId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/chat/{createContent.Data.ConversationId}/members/{nonExistentMemberId}", token);
        ApiResponse<RemoveMemberFromGroupCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RemoveMemberFromGroupCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task RemoveMemberFromGroup_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        Guid memberId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/chat/{conversationId}/members/{memberId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task MarkMessageAsDeleted_WithValidMessageId_ReturnsNoContent()
    {
        // Arrange
        string creatorEmail = $"delete-message-creator-{Guid.NewGuid():N}@omega.com";
        string memberEmail = $"delete-message-member-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(creatorEmail, "Delete Message Creator", TestConstants.DEFAULT_PASSWORD);
        RegisterNewUserCommandResult memberUser = await RegisterUserAsync(memberEmail, "Delete Message Member", TestConstants.DEFAULT_PASSWORD);

        string creatorToken = await AuthenticateAsync(creatorEmail, TestConstants.DEFAULT_PASSWORD);
        string memberToken = await AuthenticateAsync(memberEmail, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Delete Message Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, creatorToken);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        object addMemberRequest = new { UserId = memberUser.UserId };
        await PostAsync($"/api/chat/{createContent.Data.ConversationId}/members", addMemberRequest, creatorToken);

        object messageRequest = new { Type = MessageType.Normal, Body = "Message to delete" };
        HttpResponseMessage sendResponse = await PostAsync($"/api/chat/{createContent.Data.ConversationId}/messages", messageRequest, creatorToken);
        ApiResponse<SendMessageCommandResult> sendContent = await sendResponse.Content.ReadFromJsonAsync<ApiResponse<SendMessageCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Wait for async event processing to create MemberMessage records
        await Task.Delay(5000);

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/chat/{createContent.Data.ConversationId}/messages/{sendContent.Data.MessageId}", memberToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task MarkMessageAsDeleted_WithNonExistentMessageId_ReturnsNotFound()
    {
        // Arrange
        string email = $"delete-msg-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Delete Msg Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object createRequest = new { GroupName = "Delete Msg Nonexistent Test", MaxNumberOfMembers = (byte)10 };
        HttpResponseMessage createResponse = await PostAsync("/api/chat", createRequest, token);
        ApiResponse<CreateGroupConversationCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateGroupConversationCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        Guid nonExistentMessageId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/chat/{createContent.Data.ConversationId}/messages/{nonExistentMessageId}", token);
        ApiResponse<MarkMessageAsDeletedCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<MarkMessageAsDeletedCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task MarkMessageAsDeleted_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid conversationId = Guid.NewGuid();
        Guid messageId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/chat/{conversationId}/messages/{messageId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
