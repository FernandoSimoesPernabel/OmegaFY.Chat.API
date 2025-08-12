using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;

namespace OmegaFY.Chat.API.Common.Extensions;

public static class ExceptionExtensions
{
    public static string GetSafeErrorMessageWhenInProd(this Exception ex, bool isDevelopment) => isDevelopment ? ex.ToString() : "Ops ¯ _ (ツ) _ / ¯";

    public static string GetErrorCode(this Exception ex) 
        => ex is DomainErrorCodeException errorCodeException ? errorCodeException.ErrorCode : ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR;
}