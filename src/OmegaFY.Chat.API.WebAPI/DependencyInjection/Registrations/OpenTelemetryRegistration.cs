using OmegaFY.Chat.API.Infra.DependencyInjection;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class OpenTelemetryRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder) => builder.Services.AddOpenTelemetry(builder.Configuration);
}