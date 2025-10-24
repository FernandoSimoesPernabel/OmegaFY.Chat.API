using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class RateLimiterRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder) => builder.Services.AddWebApiRateLimiter(builder.Configuration);
}