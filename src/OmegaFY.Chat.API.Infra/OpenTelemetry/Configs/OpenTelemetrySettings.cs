namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;

internal record class OpenTelemetrySettings
{
    public string ServiceName { get; set; }

    public string HoneycombApiKey { get; set; }

    public string HoneycombApiKeyHeader => $"x-honeycomb-team={HoneycombApiKey}";

    public string HoneycombUrl { get; set; }
}