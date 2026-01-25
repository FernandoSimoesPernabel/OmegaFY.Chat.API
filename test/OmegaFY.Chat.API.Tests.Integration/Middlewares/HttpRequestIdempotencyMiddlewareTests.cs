using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Tests.Integration.Base;
using OmegaFY.Chat.API.Tests.Integration.Constants;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.Tests.Integration.Middlewares;

public class HttpRequestIdempotencyMiddlewareTests : IntegrationTestBase
{
    public HttpRequestIdempotencyMiddlewareTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task PostRequest_WithoutIdempotencyKey_ReturnsBadRequest()
    {
        // Arrange
        object request = new
        {
            Email = $"test-{Guid.NewGuid():N}@omega.com",
            DisplayName = "Test User",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", request, addIdempotencyKey: false);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostRequest_WithIdempotencyKey_ReturnsSuccess()
    {
        // Arrange
        string idempotencyKey = Guid.NewGuid().ToString();
        object request = new
        {
            Email = $"test-{Guid.NewGuid():N}@omega.com",
            DisplayName = "Test User",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act
        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", request, idempotencyKey: idempotencyKey);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostRequest_WithDuplicateIdempotencyKey_ReturnsConflict()
    {
        // Arrange
        string idempotencyKey = Guid.NewGuid().ToString();
        object firstRequest = new
        {
            Email = $"test-{Guid.NewGuid():N}@omega.com",
            DisplayName = "Test User",
            Password = TestConstants.DEFAULT_PASSWORD
        };
        
        object secondRequest = new
        {
            Email = $"test-{Guid.NewGuid():N}@omega.com",
            DisplayName = "Another User",
            Password = TestConstants.DEFAULT_PASSWORD
        };

        // Act - First request
        HttpResponseMessage firstResponse = await PostAsync("/api/auth/register-new-user", firstRequest, idempotencyKey: idempotencyKey);
        
        // Wait a bit to ensure cache is set
        await Task.Delay(100);
        
        // Act - Second request with same idempotency key but different payload
        HttpResponseMessage secondResponse = await PostAsync("/api/auth/register-new-user", secondRequest, idempotencyKey: idempotencyKey);

        // Assert
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task GetRequest_WithoutIdempotencyKey_ReturnsSuccess()
    {
        // Arrange
        string email = $"test-{Guid.NewGuid():N}@omega.com";
        string password = TestConstants.DEFAULT_PASSWORD;
        await RegisterUserAsync(email, "Test User", password);
        string token = await AuthenticateAsync(email, password);

        // Act - GET request without idempotency key
        HttpResponseMessage response = await GetAsync("/api/users/me", token, addIdempotencyKey: false);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutRequest_WithoutIdempotencyKey_ReturnsBadRequest()
    {
        // Arrange
        string email = $"test-{Guid.NewGuid():N}@omega.com";
        string password = TestConstants.DEFAULT_PASSWORD;
        await RegisterUserAsync(email, "Test User", password);
        string token = await AuthenticateAsync(email, password);

        object request = new { DisplayName = "Updated Name" };

        // Act
        HttpResponseMessage response = await PutAsync("/api/users/change-display-name", request, token, addIdempotencyKey: false);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteRequest_WithoutIdempotencyKey_ReturnsBadRequest()
    {
        // Arrange
        string email = $"test-{Guid.NewGuid():N}@omega.com";
        string password = TestConstants.DEFAULT_PASSWORD;
        await RegisterUserAsync(email, "Test User", password);
        string token = await AuthenticateAsync(email, password);

        // Act
        HttpResponseMessage response = await DeleteAsync("/api/auth/logoff", token, addIdempotencyKey: false);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
