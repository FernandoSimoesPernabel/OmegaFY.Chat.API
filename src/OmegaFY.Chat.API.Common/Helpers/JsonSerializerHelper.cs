using System.Text.Json;
using System.Text.Json.Serialization;

namespace OmegaFY.Chat.API.Common.Helpers;

public static class JsonSerializerHelper
{
    private static readonly JsonSerializerOptions SERIALIZER_OPTIONS = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static T Deserialize<T>(string jsonValue) => JsonSerializer.Deserialize<T>(jsonValue, SERIALIZER_OPTIONS);

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, SERIALIZER_OPTIONS);
}