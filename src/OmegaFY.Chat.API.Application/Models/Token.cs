namespace OmegaFY.Chat.API.Application.Models;

public readonly record struct Token
{
    public string Value { get; init; }

    public DateTime ExpirationDate { get; init; }

    public Token() { }

    public Token(string value, DateTime expirationDate)
    {
        Value = value;
        ExpirationDate = expirationDate;
    }
}