namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed record class RegisterNewUserCommandResult : ICommandResult
{
    public Guid UserId { get; init; }

    public string Token { get; init; }

    public DateTime TokenExpirationDate { get; init; }

    public Guid RefreshToken { get; init; }

    public DateTime RefreshTokenExpirationDate { get; init; }

    public RegisterNewUserCommandResult(Guid userId, string token, DateTime tokenExpirationDate, Guid refreshToken, DateTime refreshTokenExpirationDate)
    {
        UserId = userId;
        Token = token;
        TokenExpirationDate = tokenExpirationDate;
        RefreshToken = refreshToken;
        RefreshTokenExpirationDate = refreshTokenExpirationDate;
    }
}