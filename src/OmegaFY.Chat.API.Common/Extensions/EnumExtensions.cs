namespace OmegaFY.Chat.API.Common.Extensions;

public static class EnumExtensions
{
    public static bool IsDefined<TEnum>(this TEnum @enum) where TEnum : struct, Enum => Enum.IsDefined(@enum);
}