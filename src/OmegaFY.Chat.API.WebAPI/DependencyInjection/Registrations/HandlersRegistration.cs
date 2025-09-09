using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Infra.Extensions;

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