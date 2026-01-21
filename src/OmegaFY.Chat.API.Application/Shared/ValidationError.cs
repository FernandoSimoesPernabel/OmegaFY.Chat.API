namespace OmegaFY.Chat.API.Application.Shared;

public sealed record class ValidationError
{
    public string Code { get; init; }

    public string Message { get; init; }

    public ValidationError() { }

    public ValidationError(string code, string message)
    {
        Code = code ?? string.Empty;
        Message = message ?? string.Empty;
    }
}