namespace OmegaFY.Chat.API.Infra.Authentication.Models;

public readonly record struct AuthenticationToken
{
    public string Token { get; }

    public DateTime TokenExpirationDate { get; }

    public string RefreshToken { get; }

    public DateTime RefreshTokenExpirationDate { get; }

    public AuthenticationToken(string token, DateTime tokenExpirationDate, DateTime refreshTokenExpirationDate)
        : this(token, tokenExpirationDate, Guid.NewGuid().ToString(), refreshTokenExpirationDate) { }

    [System.Text.Json.Serialization.JsonConstructor]
    public AuthenticationToken(string token, DateTime tokenExpirationDate, string refreshToken, DateTime refreshTokenExpirationDate)
    {
        Token = token;
        TokenExpirationDate = tokenExpirationDate;
        RefreshToken = refreshToken;
        RefreshTokenExpirationDate = refreshTokenExpirationDate;
    }
}