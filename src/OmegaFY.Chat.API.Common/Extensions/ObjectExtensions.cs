using OmegaFY.Chat.API.Common.Helpers;

namespace OmegaFY.Chat.API.Common.Extensions;

public static class ObjectExtensions
{
    public static bool In<T>(this T value, params T[] valuesToCompare)
    {
        if (valuesToCompare is null || valuesToCompare.IsEmpty()) return false;

        return valuesToCompare.Any(item => item.Equals(value));
    }

    public static string ToJson(this object value) => JsonSerializerHelper.Serialize(value);
}