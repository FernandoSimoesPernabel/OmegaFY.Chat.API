using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

namespace OmegaFY.Chat.API.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<RegisterNewUserCommandHandler>();

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        return services;
    }
}