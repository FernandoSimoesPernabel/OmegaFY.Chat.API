using OmegaFY.Chat.API.Data.EF.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class HealthCheckRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks().AddSqlServerHealthCheck(builder.Configuration);
    }
}
