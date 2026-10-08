using OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

namespace OmegaFY.Chat.API.WebAPI.Models.Auth;

public sealed record class RefreshTokenRequest
{
    public Guid UserId { get; init; }

    public string CurrentToken { get; init; }

    public string RefreshToken { get; init; }

    public RefreshTokenCommand ToCommand() => new RefreshTokenCommand(UserId, CurrentToken, RefreshToken);
}