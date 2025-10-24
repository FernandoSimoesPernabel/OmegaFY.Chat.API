using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Data.Dapper.QueryProviders.Users;
using System.Data;

namespace OmegaFY.Chat.API.Data.Dapper.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDapperQueryProviders(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddTransient<IDbConnection>(_ => new SqlConnection(configuration.GetConnectionString("AzureSql")));

        services.AddScoped<IUserQueryProvider, UserQueryProvider>();

        return services;
    }
}