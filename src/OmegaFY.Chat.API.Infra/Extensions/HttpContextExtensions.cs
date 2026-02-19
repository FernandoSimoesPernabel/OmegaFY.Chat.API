using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.Constants;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class HttpContextExtensions
{
    public static string GetRequestHeaderByName(this HttpContext httpContext, string headerName)
    {
        if (!httpContext.Request.Headers.TryGetValue(headerName, out StringValues result))
            return string.Empty;

        return result;
    }

    public static string GetAccessTokenFromQueryString(this HttpContext httpContext)
    {
        string accessToken = httpContext.GetRequestHeaderByName(QueryStringConstants.ACCESS_TOKEN);
        return string.IsNullOrWhiteSpace(accessToken) ? null : accessToken;
    }

    public static string GetRemoteIpAddress(this HttpContext httpContext)
    {
        if (httpContext.Connection.RemoteIpAddress is null) return string.Empty;

        if (httpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
            return httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

        return httpContext.Connection.RemoteIpAddress.ToString();
    }

    public static string GetUserAgent(this HttpContext httpContext) => httpContext.Request.Headers.UserAgent;

    public static string GetOriginOrReferer(this HttpContext httpContext)
    {
        string origin = httpContext.Request.Headers.Origin;

        if (!string.IsNullOrWhiteSpace(origin))
            return origin;

        string referer = httpContext.Request.Headers.Referer;

        if (!string.IsNullOrWhiteSpace(referer))
            return referer;

        return string.Empty;
    }

    public static string GenerateFingerprint(this HttpContext httpContext)
    {
        string fingerprint = $"{httpContext.GetRemoteIpAddress()}-{httpContext.GetUserAgent()}-{httpContext.GetOriginOrReferer()}";
        return MD5Helper.ComputeStringHashFromString(fingerprint);
    }

    public static bool IsSignalRHubRequest(this HttpContext httpContext) => httpContext.Request.Path.StartsWithSegments(SignalRHubConstants.HUB_PATH);
}