namespace OmegaFY.Chat.API.Infra.RateLimite.Configs;

public record class IpOrUserTokenBucketPolicySettings
{
    public TimeSpan ReplenishmentPeriod { get; set; }

    public int TokenLimit { get; set; }

    public int TokensPerPeriod { get; set; }
}
