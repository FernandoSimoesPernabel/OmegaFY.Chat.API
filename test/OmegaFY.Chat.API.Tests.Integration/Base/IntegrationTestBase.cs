using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.WebAPI.Models;
using System.Net.Http.Headers;

namespace OmegaFY.Chat.API.Tests.Integration.Base;

[Collection(nameof(IntegrationTestCollection))]
public abstract class IntegrationTestBase
{
    protected readonly CustomWebApplicationFactory _webApplicationFactory;
    
    protected readonly IMessageBus _messageBus;

    protected IntegrationTestBase(CustomWebApplicationFactory webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
        _messageBus = webApplicationFactory.Services.GetRequiredService<IMessageBus>();
    }

    protected async Task WaitQueueToProcessAsync()
    {
        while (_messageBus.GetMessageCount() > 0)
            await Task.Delay(100);

        await Task.Delay(5000);
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
        object loginRequest = new { Email = email, Password = password, RememberMe = true };

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

    protected async Task<HttpResponseMessage> GetAsync(string url) => await GetAsync(url, null);

    protected async Task<HttpResponseMessage> GetAsync(string url, string bearerToken) 
        => await CreateHttpClient(bearerToken).GetAsync(url);

    protected async Task<HttpResponseMessage> PostAsync(string url, object payload) => await PostAsync(url, payload, null);

    protected async Task<HttpResponseMessage> PostAsync(string url, object payload, string bearerToken) 
        => await CreateHttpClient(bearerToken).PostAsJsonAsync(url, payload);

    protected async Task<HttpResponseMessage> DeleteAsync(string url) => await DeleteAsync(url, null, null);

    protected async Task<HttpResponseMessage> DeleteAsync(string url, object payload, string bearerToken)
    {
        HttpClient httpClient = CreateHttpClient(bearerToken);

        if (payload is null)
            return await httpClient.DeleteAsync(url);

        return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, url)
        {
            Content = JsonContent.Create(payload)
        });
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url, string bearerToken)
        => await CreateHttpClient(bearerToken).DeleteAsync(url);

    protected async Task<HttpResponseMessage> PutAsync(string url, object payload) => await PutAsync(url, payload, null);

    protected async Task<HttpResponseMessage> PutAsync(string url, object payload, string bearerToken)
        => await CreateHttpClient(bearerToken).PutAsJsonAsync(url, payload);

    private HttpClient CreateHttpClient(string bearerToken)
    {
        HttpClient httpClient = _webApplicationFactory.CreateClient();

        if (bearerToken is not null)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        httpClient.Timeout = TimeSpan.FromSeconds(10);

        return httpClient;
    }
}