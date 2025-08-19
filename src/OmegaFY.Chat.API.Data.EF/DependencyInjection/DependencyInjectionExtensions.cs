using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Data.EF.Authentication.Services;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Infra.Authentication.Services;

namespace OmegaFY.Chat.API.Data.EF.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder identityBuilder) 
        => identityBuilder.AddEntityFrameworkStores<QueryContext>();

    public static IServiceCollection AddEntityFrameworkUserManager(this IServiceCollection services) 
        => services.AddScoped<IUserManager, EntityFrameworkUserManager>();
}