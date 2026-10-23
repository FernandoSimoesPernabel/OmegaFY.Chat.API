namespace OmegaFY.Chat.API.Application.Models;

public readonly record struct Token
{
    public Guid UserId { get; init; }

    public string Value { get; init; }

    public DateTime ExpirationDate { get; init; }

    public Token() { }

    public Token(Guid userId, string value, DateTime expirationDate)
    {
        UserId = userId;
        Value = value;
        ExpirationDate = expirationDate;
    }
}