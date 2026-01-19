using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.WebAPI.Models;
using System.Net.Http.Headers;

namespace OmegaFY.Chat.API.Tests.Integration.Base;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    protected readonly CustomWebApplicationFactory _webApplicationFactory;

    protected IntegrationTestBase(CustomWebApplicationFactory webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task<RegisterNewUserCommandResult> RegisterUserAsync(string email, string displayName, string password)
    {
        object registerRequest = new { Email = email, DisplayName = displayName, Password = password };

        HttpResponseMessage response = await PostAsync("/api/auth/register-new-user", registerRequest);
        
        response.EnsureSuccessStatusCode();

        ApiResponse<RegisterNewUserCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterNewUserCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);
        
        return content?.Data ?? throw new InvalidOperationException("Failed to register user");
    }

    protected async Task<string> AuthenticateAsync(string email, string password)
    {
        object loginRequest = new { Email = email, Password = password };

        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);
        
        response.EnsureSuccessStatusCode();

        ApiResponse<LoginCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<LoginCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);
        
        return content?.Data?.Token.Value ?? throw new InvalidOperationException("Failed to get access token");
    }

    protected async Task<(string Token, string RefreshToken)> AuthenticateWithRefreshTokenAsync(string email, string password)
    {
        object loginRequest = new { Email = email, Password = password, RememberMe = true };

        HttpResponseMessage response = await PostAsync("/api/auth/login", loginRequest);

        response.EnsureSuccessStatusCode();

        ApiResponse<LoginCommandResult> content = await response.Content.ReadFromJsonAsync<ApiResponse<LoginCommandResult>>(JsonSerializerConstants.SERIALIZER_OPTIONS);

        return (
            content?.Data?.Token.Value ?? throw new InvalidOperationException("Failed to get access token"),
            content?.Data?.RefreshToken?.Value ?? throw new InvalidOperationException("Failed to get refresh token")
        );
    }

    protected async Task<HttpResponseMessage> PostAsync(string url, object payload) => await PostAsync(url, payload, null);

    protected async Task<HttpResponseMessage> PostAsync(string url, object payload, string bearerToken)
    {
        if (bearerToken is not null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        return await _httpClient.PostAsJsonAsync(url, payload);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url) => await DeleteAsync(url, null, null);

    protected async Task<HttpResponseMessage> DeleteAsync(string url, object payload, string bearerToken)
    {
        if (bearerToken is not null)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        if (payload is null)
            return await _httpClient.DeleteAsync(url);

        return await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = JsonContent.Create(payload)
        });
    }
}