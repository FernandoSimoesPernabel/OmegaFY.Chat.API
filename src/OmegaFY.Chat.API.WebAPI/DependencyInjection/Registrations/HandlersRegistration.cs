
using OmegaFY.Chat.API.Application.DependencyInjection;
using OmegaFY.Chat.API.Infra.DependencyInjection;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class HandlersRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddInMemoryMessageBus();

        builder.Services.AddCommandHandlers();

        builder.Services.AddQueryHandlers();

        builder.Services.AddEventHandlers();
    }
}