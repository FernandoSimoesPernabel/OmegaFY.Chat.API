namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;

internal sealed record HoneycombSettings
{
    public string HoneycombApiKey { get; set; }
}