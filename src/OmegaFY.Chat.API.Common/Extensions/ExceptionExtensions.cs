namespace OmegaFY.Chat.API.Common.Extensions;

public static class ExceptionExtensions
{
    public static string GetSafeErrorMessageWhenInProd(this Exception ex, bool isDevelopment) => isDevelopment ? ex.ToString() : "Ops ¯ _ (ツ) _ / ¯";
}