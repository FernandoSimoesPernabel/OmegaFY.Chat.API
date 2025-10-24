using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed record class RegisterNewUserCommandResult : ICommandResult
{
    public Guid UserId { get; init; }

    public Token Token { get; init; }

    public Token RefreshToken { get; init; }

    public RegisterNewUserCommandResult(Guid userId, Token token, Token refreshToken)
    {
        UserId = userId;
        Token = token;
        RefreshToken = refreshToken;
    }
}