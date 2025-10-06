using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

public sealed record class RefreshTokenCommandResult : ICommandResult
{
    public Token Token { get; init; }

    public Token RefreshToken { get; init; }

    public RefreshTokenCommandResult() { }

    public RefreshTokenCommandResult(Token token, Token refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}