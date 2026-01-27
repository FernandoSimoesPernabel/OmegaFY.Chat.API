using Microsoft.AspNetCore.Http;
using OmegaFY.Chat.API.Infra.Constants;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class HttpContextExtensions
{
    public static string GetAccessTokenFromQueryString(this HttpContext httpContext)
    {
        string accessToken = httpContext.Request.Query[QueryStringConstants.ACCESS_TOKEN];
        return string.IsNullOrWhiteSpace(accessToken) ? null : accessToken;
    }

    public static bool IsSignalRHubRequest(this HttpContext httpContext) => httpContext.Request.Path.StartsWithSegments(SignalRHubConstants.HUB_PATH);
}