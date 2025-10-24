namespace OmegaFY.Chat.API.Application.Models;

public readonly record struct Token
{
    public string Value { get; }

    public DateTime ExpirationDate { get; }

    public Token(string value, DateTime expirationDate)
    {
        Value = value;
        ExpirationDate = expirationDate;
    }
}