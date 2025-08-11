namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;

internal sealed record class OpenTelemetrySettings
{
    public string ServiceName { get; set; }

    public HoneycombSettings HoneycombSettings { get; set; }
}