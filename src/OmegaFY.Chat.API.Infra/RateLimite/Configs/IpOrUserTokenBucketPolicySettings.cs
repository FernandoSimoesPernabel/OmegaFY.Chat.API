namespace OmegaFY.Chat.API.Infra.RateLimite.Configs;

public record class IpOrUserTokenBucketPolicySettings
{
    public TimeSpan ReplenishmentPeriod { get; set; }

    public int UserTokenLimit { get; set; }

    public int UserTokensPerPeriod { get; set; }

    public int IpTokenLimit { get; set; }

    public int IpTokensPerPeriod { get; set; }
}
