
namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class OpenApiRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
    }
}