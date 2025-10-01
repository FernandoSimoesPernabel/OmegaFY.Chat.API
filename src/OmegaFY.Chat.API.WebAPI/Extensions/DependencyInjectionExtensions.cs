using OmegaFY.Chat.API.WebAPI.DependencyInjection;

namespace OmegaFY.Chat.API.WebAPI.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjectionRegister(this IServiceCollection services, WebApplicationBuilder builder)
    {
        typeof(Program).Assembly?.ExportedTypes
            .Where(type => typeof(IDependencyInjectionRegister).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IDependencyInjectionRegister>()
            .ToList()
            .ForEach(iocRegister => iocRegister.Register(builder));

        return services;
    }
}