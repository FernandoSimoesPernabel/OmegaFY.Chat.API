namespace OmegaFY.Chat.API.WebAPI.Models.Configs;

public sealed record class CorsSettings
{
    public string[] AllowedOrigins { get; init; } = [];

    public string[] AllowedMethods { get; init; } = [];

    public string[] AllowedHeaders { get; init; } = [];
}