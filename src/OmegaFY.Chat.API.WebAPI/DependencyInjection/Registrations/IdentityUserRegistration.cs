using OmegaFY.Chat.API.Infra.DependencyInjection;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class IdentityUserRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityUserConfiguration(builder.Configuration);

        //builder.Services.AddAuthenticationSettings(builder.Configuration);

        //builder.Services.AddEntityFrameworkUserManager();
        //builder.Services.AddIdentity().AddEntityFrameworkStores().AddDefaultTokenProviders();
    }
}