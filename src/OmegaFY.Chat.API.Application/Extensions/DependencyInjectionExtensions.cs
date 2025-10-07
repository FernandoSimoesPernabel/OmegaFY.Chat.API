using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Application.Commands.Auth.Logoff;
using OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Events;
using OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services) 
        => services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtensions).Assembly);

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<RegisterNewUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<LogoffCommandHandler>();
        services.AddScoped<RefreshTokenCommandHandler>();

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.AddScoped<GetCurrentUserInfoQueryHandler>();

        return services;
    }

    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IEventHandler<UserRegisteredEvent>, SendWelcomeEmailEventHandler>();

        return services;
    }
}