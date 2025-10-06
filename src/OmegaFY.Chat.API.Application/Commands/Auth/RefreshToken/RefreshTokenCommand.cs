namespace OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

public sealed record class RefreshTokenCommand : ICommand
{
    public string CurrentToken { get; init; }

    public string RefreshToken { get; init; }

    public RefreshTokenCommand() { }

    public RefreshTokenCommand(string currentToken, string refreshToken)
    {
        CurrentToken = currentToken;
        RefreshToken = refreshToken;
    }
}