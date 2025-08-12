namespace OmegaFY.Chat.API.Infra.Authentication.Models;

internal sealed class JwtSettings
{
    public string Audience { get; set; }

    public string Issuer { get; set; }

    public string Secret { get; set; }

    public TimeSpan TimeToExpireToken { get; set; }

    public TimeSpan TimeToExpireRefreshToken { get; set; }
}