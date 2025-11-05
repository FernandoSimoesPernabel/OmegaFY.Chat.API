using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OmegaFY.Chat.API.Data.EF.Extensions;

public static class HealthCheckServiceCollectionExtensions
{
    public static IHealthChecksBuilder AddSqlServerHealthCheck(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration) 
        => healthChecksBuilder.AddSqlServer(configuration.GetConnectionString("AzureSql"), name: "SqlServer", tags: ["database", "storage", "sql"]);
}