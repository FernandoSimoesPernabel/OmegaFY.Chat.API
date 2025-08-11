namespace OmegaFY.Chat.API.Common.Exceptions.Constants;

public static class ApplicationErrorCodesConstants
{
    public const string UNAUTHORIZED = nameof(UNAUTHORIZED);

    public const string DOMAIN_ARGUMENT_INVALID = nameof(DOMAIN_ARGUMENT_INVALID);

    public const string GENERIC_DOMAIN_ERROR = nameof(GENERIC_DOMAIN_ERROR);

    public const string INVALID_OPERATION = nameof(INVALID_OPERATION);

    public const string NOT_DOMAIN_ERROR = nameof(NOT_DOMAIN_ERROR);

    public const string NOT_FOUND = nameof(NOT_FOUND);

    public const string ENTITY_CONFLICTED = nameof(ENTITY_CONFLICTED);

    public const string UNABLE_TO_CREATE_USER_ON_IDENTITY = nameof(UNABLE_TO_CREATE_USER_ON_IDENTITY);
}