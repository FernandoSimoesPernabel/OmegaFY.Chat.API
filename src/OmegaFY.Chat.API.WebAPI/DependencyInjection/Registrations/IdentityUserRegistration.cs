using Microsoft.AspNetCore.Identity;
using OmegaFY.Chat.API.Data.EF.Extensions;
using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class IdentityUserRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityUserConfiguration(builder.Configuration);

        builder.Services.AddEntityFrameworkUserManager();

        builder.Services.AddIdentity().AddEntityFrameworkStores().AddDefaultTokenProviders();
    }
}