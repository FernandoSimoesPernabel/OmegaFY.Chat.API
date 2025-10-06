using OmegaFY.Chat.API.Application.Models;

namespace OmegaFY.Chat.API.Application.Commands.Auth.Login;

public sealed record class LoginCommandResult : ICommandResult
{
    public Guid UserId { get; init; }

    public string DisplayName { get; init; }

    public string Email { get; init; }

    public Token Token { get; init; }

    public Token? RefreshToken { get; init; }

    public LoginCommandResult() { }

    public LoginCommandResult(Guid userId, string displayName, string email, Token token, Token? refreshToken)
    {
        UserId = userId;
        DisplayName = displayName;
        Email = email;
        Token = token;
        RefreshToken = refreshToken;
    }
}