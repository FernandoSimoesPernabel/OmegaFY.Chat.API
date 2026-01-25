using OmegaFY.Chat.API.WebAPI.Middlewares;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class MiddlewareRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder) 
        => builder.Services.AddSingleton<HttpRequestIdempotencyMiddleware>();
}
