using OmegaFY.Chat.API.WebAPI.BackgroundServices;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class BackgroundServiceRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<ChatEventsQueueConsumerBackgroundService>();
    }
}