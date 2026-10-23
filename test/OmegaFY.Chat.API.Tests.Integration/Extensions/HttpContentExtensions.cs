using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.Tests.Integration.Extensions;

internal static class HttpContentExtensions
{
    public static async Task<ApiResponse<T>> ReadApiResponseAsync<T>(this HttpContent content) => await content.ReadFromJsonAsync<ApiResponse<T>>(JsonSerializerConstants.SERIALIZER_OPTIONS, TestContext.Current.CancellationToken);
}