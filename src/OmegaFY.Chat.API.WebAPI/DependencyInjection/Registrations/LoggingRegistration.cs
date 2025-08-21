
namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class LoggingRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
        });
    }
}