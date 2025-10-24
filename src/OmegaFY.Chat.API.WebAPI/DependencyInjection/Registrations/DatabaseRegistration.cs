using OmegaFY.Chat.API.Data.Dapper.Extensions;
using OmegaFY.Chat.API.Data.EF.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class DatabaseRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddSqlServerEntityFrameworkContexts(builder.Configuration, builder.Environment);
        
        builder.Services.AddEntityFrameworkRepositories();
        
        builder.Services.AddDapperQueryProviders(builder.Configuration);
    }
}