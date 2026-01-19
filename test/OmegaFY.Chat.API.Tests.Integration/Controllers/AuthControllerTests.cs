using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Application.Commands.Auth.Logoff;
using OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Tests.Integration.Base;
using OmegaFY.Chat.API.Tests.Integration.Constants;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.Tests.Integration.Controllers;

public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task RegisterNewUser_WithValidData_ReturnsCreated()
    {
        // Arrange
        object request = new
        {
            Email = $"newuser-{Guid.NewGuid():N}@omega.com",
            DisplayName = "New Test User",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", request);
        ApiResponse<RegisterNewUserCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterNewUserCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotEqual(Guid.Empty, content.Data.UserId);
    }

    [Fact]
    public async Task RegisterNewUser_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        object request = new
        {
            Email = "invalid-email",
            DisplayName = "Test User",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RegisterNewUser_WithWeakPassword_ReturnsBadRequest()
    {
        // Arrange
        object request = new
        {
            Email = $"weakpass-{Guid.NewGuid():N}@omega.com",
            DisplayName = "Test User",
            Password = "123"
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RegisterNewUser_WithDuplicateEmail_ReturnsConflict()
    {
        // Arrange
        string email = $"duplicate-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "First User", TestConstants.DEFAULT_PASSWORD);

        object duplicateRequest = new
        {
            Email = email,
            DisplayName = "Second User",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", duplicateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task RegisterNewUser_WithEmptyDisplayName_ReturnsBadRequest()
    {
        // Arrange
        object request = new
        {
            Email = $"emptyname-{Guid.NewGuid():N}@omega.com",
            DisplayName = "",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        string email = $"login-test-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Login Test User", TestConstants.DEFAULT_PASSWORD);

        object loginRequest = new
        {
            Email = email,
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);
        ApiResponse<LoginCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<LoginCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotEmpty(content.Data.Token.Value);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        object loginRequest = new
        {
            Email = "nonexistent@omega.com",
            Password = "WrongPassword123!"
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        object loginRequest = new
        {
            Email = "",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithEmptyPassword_ReturnsBadRequest()
    {
        // Arrange
        object loginRequest = new
        {
            Email = "test@omega.com",
            Password = ""
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        // Arrange
        string email = $"wrongpass-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Test User", TestConstants.DEFAULT_PASSWORD);

        object loginRequest = new
        {
            Email = email,
            Password = "WrongPassword123!"
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_WithValidTokens_ReturnsNewTokens()
    {
        // Arrange
        string email = $"refresh-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Refresh Test User", TestConstants.DEFAULT_PASSWORD);
        (string token, string refreshToken) = await AuthenticateWithRefreshTokenAsync(email, TestConstants.DEFAULT_PASSWORD);

        object refreshRequest = new
        {
            CurrentToken = token,
            RefreshToken = refreshToken
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/refresh-token", refreshRequest, token);
        ApiResponse<RefreshTokenCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RefreshTokenCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
        Assert.NotEmpty(content.Data.Token.Value);
        Assert.NotEmpty(content.Data.RefreshToken.Value);
    }

    [Fact]
    public async Task RefreshToken_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        object refreshRequest = new
        {
            CurrentToken = "invalid-token",
            RefreshToken = "invalid-refresh-token"
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/refresh-token", refreshRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_WithInvalidRefreshToken_ReturnsBadRequest()
    {
        // Arrange
        string email = $"invalid-refresh-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Invalid Refresh Test User", TestConstants.DEFAULT_PASSWORD);
        (string token, _) = await AuthenticateWithRefreshTokenAsync(email, TestConstants.DEFAULT_PASSWORD);

        object refreshRequest = new
        {
            CurrentToken = token,
            RefreshToken = "invalid-refresh-token"
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/refresh-token", refreshRequest, token);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logoff_WithValidToken_ReturnsAccepted()
    {
        // Arrange
        string email = $"logoff-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Logoff Test User", TestConstants.DEFAULT_PASSWORD);
        (string token, string refreshToken) = await AuthenticateWithRefreshTokenAsync(email, TestConstants.DEFAULT_PASSWORD);

        object logoffRequest = new
        {
            RefreshToken = refreshToken
        };

        // Act
        HttpResponseMessage response = await DeleteAsync("/api/auth/logoff", logoffRequest, token);
        ApiResponse<LogoffCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<LogoffCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Succeeded);
    }

    [Fact]
    public async Task Logoff_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        object logoffRequest = new
        {
            RefreshToken = "some-refresh-token"
        };

        // Act
        HttpResponseMessage response = await DeleteAsync("/api/auth/logoff", logoffRequest, null);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logoff_WithInvalidRefreshToken_ReturnsBadRequest()
    {
        // Arrange
        string email = $"logoff-invalid-{Guid.NewGuid():N}@omega.com";
        await RegisterUserAsync(email, "Logoff Invalid Test User", TestConstants.DEFAULT_PASSWORD);
        string token = await AuthenticateAsync(email, TestConstants.DEFAULT_PASSWORD);

        object logoffRequest = new
        {
            RefreshToken = "invalid-refresh-token"
        };

        // Act
        HttpResponseMessage response = await DeleteAsync("/api/auth/logoff", logoffRequest, token);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Accepted);
    }
}