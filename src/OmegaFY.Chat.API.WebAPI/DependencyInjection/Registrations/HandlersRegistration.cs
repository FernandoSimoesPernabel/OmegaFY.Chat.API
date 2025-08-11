
using OmegaFY.Chat.API.Application.DependencyInjection;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class HandlersRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddCommandHandlers();

        builder.Services.AddQueryHandlers();

        builder.Services.AddEventHandlers();
    }
}