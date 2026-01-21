using System.Text.Json;
using System.Text.Json.Serialization;

namespace OmegaFY.Chat.API.Common.Constants;

public static class JsonSerializerConstants
{
    public static readonly JsonSerializerOptions SERIALIZER_OPTIONS = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}