using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class CacheRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder) => builder.Services.AddCache();
}