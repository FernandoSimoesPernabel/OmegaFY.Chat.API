using OmegaFY.Chat.API.Common.Constants;
using System.Text.Json;

namespace OmegaFY.Chat.API.Common.Helpers;

public static class JsonSerializerHelper
{
    public static T Deserialize<T>(string jsonValue) => JsonSerializer.Deserialize<T>(jsonValue, JsonSerializerConstants.SERIALIZER_OPTIONS);

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, JsonSerializerConstants.SERIALIZER_OPTIONS);
}