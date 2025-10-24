namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;

internal sealed record HoneycombSettings
{
    public string HoneycombApiKey { get; set; }

    public Uri HoneycombApiEndpoint => new Uri("https://api.honeycomb.io");

    public string HoneycombApiKeyHeader => $"x-honeycomb-team={HoneycombApiKey}";
}