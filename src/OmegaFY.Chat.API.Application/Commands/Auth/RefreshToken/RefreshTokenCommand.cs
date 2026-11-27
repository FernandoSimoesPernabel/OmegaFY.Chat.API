namespace OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

public sealed record class RefreshTokenCommand : ICommand
{
    public Guid UserId { get; init; }

    public string CurrentToken { get; init; }

    public string RefreshToken { get; init; }

    public RefreshTokenCommand() { }

    public RefreshTokenCommand(Guid userId, string currentToken, string refreshToken)
    {
        UserId = userId;
        CurrentToken = currentToken;
        RefreshToken = refreshToken;
    }
}