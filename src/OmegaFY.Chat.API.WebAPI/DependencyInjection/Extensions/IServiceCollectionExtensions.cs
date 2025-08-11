using System.Reflection;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjectionRegister(this IServiceCollection services, WebApplicationBuilder builder)
    {
        typeof(Program).Assembly?.ExportedTypes
            .Where(t => typeof(IDependencyInjectionRegister).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IDependencyInjectionRegister>()
            .ToList()
            .ForEach(iocRegister => iocRegister.Register(builder));

        return services;
    }
}