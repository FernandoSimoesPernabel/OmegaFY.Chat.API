using OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;
using OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;
using OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;
using OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;
using OmegaFY.Chat.API.Application.Queries.Users.GetUserById;
using OmegaFY.Chat.API.Application.Queries.Users.GetUsers;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Tests.Integration.Base;
using OmegaFY.Chat.API.Tests.Integration.Constants;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.Tests.Integration.Controllers;

public class UsersControllerTests : IntegrationTestBase
{
    public UsersControllerTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetUsers_WithValidToken_ReturnsOk()
    {
        // Arrange
        string email = $"getusers-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Get Users Test User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync("/api/users", token);
        ApiResponse<GetUsersQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUsersQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Users);
    }

    [Fact]
    public async Task GetUsers_WithDisplayNameFilter_ReturnsFilteredUsers()
    {
        // Arrange
        string uniqueName = $"UniqueFilter{Guid.NewGuid():N}";
        string email = $"filter-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, uniqueName, TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users?displayName={uniqueName}", token);
        ApiResponse<GetUsersQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUsersQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotNull(content.Data);
    }

    [Fact]
    public async Task GetUsers_WithFriendshipStatusFilter_ReturnsOk()
    {
        // Arrange
        string email = $"status-filter-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Status Filter User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users?status={FriendshipStatus.Accepted}", token);
        ApiResponse<GetUsersQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUsersQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotNull(content.Data);
    }

    [Fact]
    public async Task GetUsers_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        HttpResponseMessage response = await GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUserInfo_WithValidToken_ReturnsUserInfo()
    {
        // Arrange
        string email = $"me-{Guid.NewGuid():N}@omega.com";
        string displayName = "Current User Info Test";
        await RegisterUserAsync(email, displayName, TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync("/api/users/me", token);
        ApiResponse<GetCurrentUserInfoQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetCurrentUserInfoQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotEqual(Guid.Empty, content.Data.Id);
        Assert.Equal(email, content.Data.Email);
        Assert.Equal(displayName, content.Data.DisplayName);
        Assert.NotNull(content.Data.Friendships);
    }

    [Fact]
    public async Task GetCurrentUserInfo_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Act
        HttpResponseMessage response = await GetAsync("/api/users/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_WithValidUserId_ReturnsUser()
    {
        // Arrange
        string email = $"getuserbyid-{Guid.NewGuid():N}@omega.com";
        string displayName = "Get User By Id Test";
        var registeredUser = await RegisterUserAsync(email, displayName, TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users/{registeredUser.UserId}", token);
        ApiResponse<GetUserByIdQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.User);
        Assert.Equal(registeredUser.UserId, content.Data.User.Id);
        Assert.Equal(email, content.Data.User.Email);
        Assert.Equal(displayName, content.Data.User.DisplayName);
    }

    [Fact]
    public async Task GetUserById_WithNonExistentUserId_ReturnsNotFound()
    {
        // Arrange
        string email = $"nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Test User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentUserId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users/{nonExistentUserId}", token);
        ApiResponse<GetUserByIdQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task GetUserById_WithInvalidGuidFormat_ReturnsNotFound()
    {
        // Arrange
        string email = $"invalidguid-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Test User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await GetAsync("/api/users/invalid-guid", token);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid userId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SendFriendshipRequest_WithValidInvitedUser_ReturnsCreated()
    {
        // Arrange
        string requestingEmail = $"requester-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"invited-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Requester User", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Invited User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);

        object request = new { InvitedUserId = invitedUser.UserId };

        // Act
        HttpResponseMessage response = await PostAsync("/api/users/me/friendships", request, token);
        ApiResponse<SendFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotEqual(Guid.Empty, content.Data.FriendshipId);
    }

    [Fact]
    public async Task SendFriendshipRequest_ToSelf_ReturnsBadRequest()
    {
        // Arrange
        string email = $"selfadd-{Guid.NewGuid():N}@omega.com";
        var user = await RegisterUserAsync(email, "Self Add User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object request = new { InvitedUserId = user.UserId };

        // Act
        HttpResponseMessage response = await PostAsync("/api/users/me/friendships", request, token);
        ApiResponse<SendFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task SendFriendshipRequest_ToNonExistentUser_ReturnsInternalServerError()
    {
        // Arrange
        string email = $"friendreq-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Friend Request Test User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object request = new { InvitedUserId = Guid.NewGuid() };

        // Act
        HttpResponseMessage response = await PostAsync("/api/users/me/friendships", request, token);
        ApiResponse<SendFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task SendFriendshipRequest_WithEmptyGuid_ReturnsBadRequest()
    {
        // Arrange
        string email = $"emptyguid-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Empty Guid Test User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object request = new { InvitedUserId = Guid.Empty };

        // Act
        HttpResponseMessage response = await PostAsync("/api/users/me/friendships", request, token);
        ApiResponse<SendFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task SendFriendshipRequest_DuplicateRequest_ReturnsBadRequest()
    {
        // Arrange
        string requestingEmail = $"duplicate-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"duplicate-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Duplicate Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Duplicate Invited", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);

        object request = new { InvitedUserId = invitedUser.UserId };

        // First request
        await PostAsync("/api/users/me/friendships", request, token);

        // Act - Second request (duplicate)
        HttpResponseMessage response = await PostAsync("/api/users/me/friendships", request, token);
        ApiResponse<SendFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task SendFriendshipRequest_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        object request = new { InvitedUserId = Guid.NewGuid() };

        // Act
        HttpResponseMessage response = await PostAsync("/api/users/me/friendships", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetFriendshipById_WithValidFriendshipId_ReturnsFriendship()
    {
        // Arrange
        string requestingEmail = $"getfriendship-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"getfriendship-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Get Friendship Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Get Friendship Invited", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);

        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, token);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}", token);
        ApiResponse<GetFriendshipByIdQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetFriendshipByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.Empty(content.Errors);
        Assert.NotNull(content.Data);
        Assert.NotNull(content.Data.Friendship);
        Assert.Equal(createContent.Data.FriendshipId, content.Data.Friendship.FriendshipId);
        Assert.Equal(FriendshipStatus.Pending, content.Data.Friendship.Status);
    }

    [Fact]
    public async Task GetFriendshipById_WithNonExistentFriendshipId_ReturnsNotFound()
    {
        // Arrange
        string email = $"nonexistent-friend-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Nonexistent Friendship User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentFriendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users/me/friendships/{nonExistentFriendshipId}", token);
        ApiResponse<GetFriendshipByIdQueryResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<GetFriendshipByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task GetFriendshipById_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid friendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await GetAsync($"/api/users/me/friendships/{friendshipId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AcceptFriendshipRequest_WithValidFriendshipId_ReturnsNoContent()
    {
        // Arrange
        string requestingEmail = $"accept-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"accept-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Accept Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Accept Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        string invitedToken = await AuthenticateAsync(invitedEmail, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/accept", new { }, invitedToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task AcceptFriendshipRequest_VerifyStatusChange_FriendshipIsAccepted()
    {
        // Arrange
        string requestingEmail = $"accept-verify-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"accept-verify-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Accept Verify Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Accept Verify Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        string invitedToken = await AuthenticateAsync(invitedEmail, TestConstants.DEFAULT_PASSWORD);
        await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/accept", new { }, invitedToken);

        // Act - Get friendship to verify status
        HttpResponseMessage getResponse = await GetAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}", invitedToken);
        ApiResponse<GetFriendshipByIdQueryResult> getContent = await getResponse.Content.ReadFromJsonAsync<ApiResponse<GetFriendshipByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(getContent);
        Assert.True(getContent.Succeeded);
        Assert.Equal(FriendshipStatus.Accepted, getContent.Data.Friendship.Status);
    }

    [Fact]
    public async Task AcceptFriendshipRequest_WithNonExistentFriendshipId_ReturnsNotFound()
    {
        // Arrange
        string email = $"accept-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Accept Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentFriendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{nonExistentFriendshipId}/accept", new { }, token);
        ApiResponse<AcceptFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<AcceptFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task AcceptFriendshipRequest_ByRequester_ReturnsBadRequest()
    {
        // Arrange
        string requestingEmail = $"accept-self-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"accept-self-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Accept Self Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Accept Self Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act - Requester tries to accept their own request
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/accept", new { }, requesterToken);
        ApiResponse<AcceptFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<AcceptFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task AcceptFriendshipRequest_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid friendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{friendshipId}/accept", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RejectFriendshipRequest_WithValidFriendshipId_ReturnsNoContent()
    {
        // Arrange
        string requestingEmail = $"reject-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"reject-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Reject Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Reject Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        string invitedToken = await AuthenticateAsync(invitedEmail, TestConstants.DEFAULT_PASSWORD);

        // Act
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/reject", new { }, invitedToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RejectFriendshipRequest_VerifyStatusChange_FriendshipIsRejected()
    {
        // Arrange
        string requestingEmail = $"reject-verify-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"reject-verify-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Reject Verify Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Reject Verify Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        string invitedToken = await AuthenticateAsync(invitedEmail, TestConstants.DEFAULT_PASSWORD);
        await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/reject", new { }, invitedToken);

        // Act - Get friendship to verify status
        HttpResponseMessage getResponse = await GetAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}", invitedToken);
        ApiResponse<GetFriendshipByIdQueryResult> getContent = await getResponse.Content.ReadFromJsonAsync<ApiResponse<GetFriendshipByIdQueryResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(getContent);
        Assert.True(getContent.Succeeded);
        Assert.Equal(FriendshipStatus.Rejected, getContent.Data.Friendship.Status);
    }

    [Fact]
    public async Task RejectFriendshipRequest_WithNonExistentFriendshipId_ReturnsInternalServerError()
    {
        // Arrange
        string email = $"reject-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Reject Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentFriendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{nonExistentFriendshipId}/reject", new { }, token);
        ApiResponse<RejectFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RejectFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task RejectFriendshipRequest_ByRequester_ReturnsBadRequest()
    {
        // Arrange
        string requestingEmail = $"reject-self-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"reject-self-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Reject Self Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Reject Self Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act - Requester tries to reject their own request
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/reject", new { }, requesterToken);
        ApiResponse<RejectFriendshipRequestCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RejectFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task RejectFriendshipRequest_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid friendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/users/me/friendships/{friendshipId}/reject", new { });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveFriendship_WithValidAcceptedFriendship_ReturnsNoContent()
    {
        // Arrange
        string requestingEmail = $"remove-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"remove-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Remove Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Remove Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        string invitedToken = await AuthenticateAsync(invitedEmail, TestConstants.DEFAULT_PASSWORD);
        await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/accept", new { }, invitedToken);

        // Act - Remove the friendship
        HttpResponseMessage response = await DeleteAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}", requesterToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemoveFriendship_ByInvitedUser_ReturnsNoContent()
    {
        // Arrange
        string requestingEmail = $"remove-by-inv-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"remove-by-inv-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Remove By Inv Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Remove By Inv Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        string invitedToken = await AuthenticateAsync(invitedEmail, TestConstants.DEFAULT_PASSWORD);
        await PostAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}/accept", new { }, invitedToken);

        // Act - Invited user removes the friendship
        HttpResponseMessage response = await DeleteAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}", invitedToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemoveFriendship_WithNonExistentFriendshipId_ReturnsInternalServerError()
    {
        // Arrange
        string email = $"remove-nonexistent-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Remove Nonexistent User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);
        Guid nonExistentFriendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/users/me/friendships/{nonExistentFriendshipId}", token);
        ApiResponse<RemoveFriendshipCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RemoveFriendshipCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(content);
        Assert.False(content.Succeeded);
        Assert.NotEmpty(content.Errors);
    }

    [Fact]
    public async Task RemoveFriendship_PendingFriendship_ReturnsNoContent()
    {
        // Arrange
        string requestingEmail = $"remove-pending-req-{Guid.NewGuid():N}@omega.com";
        string invitedEmail = $"remove-pending-inv-{Guid.NewGuid():N}@omega.com";

        await RegisterUserAsync(requestingEmail, "Remove Pending Requester", TestConstants.DEFAULT_PASSWORD);
        var invitedUser = await RegisterUserAsync(invitedEmail, "Remove Pending Invited", TestConstants.DEFAULT_PASSWORD);

        string requesterToken = await AuthenticateAsync(requestingEmail, TestConstants.DEFAULT_PASSWORD);
        object request = new { InvitedUserId = invitedUser.UserId };
        HttpResponseMessage createResponse = await PostAsync("/api/users/me/friendships", request, requesterToken);
        ApiResponse<SendFriendshipRequestCommandResult> createContent = await createResponse.Content.ReadFromJsonAsync<ApiResponse<SendFriendshipRequestCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Act - Remove pending friendship
        HttpResponseMessage response = await DeleteAsync($"/api/users/me/friendships/{createContent.Data.FriendshipId}", requesterToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemoveFriendship_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        Guid friendshipId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/users/me/friendships/{friendshipId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}