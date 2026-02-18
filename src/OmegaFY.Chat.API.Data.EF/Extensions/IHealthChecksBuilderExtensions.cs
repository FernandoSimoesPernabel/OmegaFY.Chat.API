using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OmegaFY.Chat.API.Data.EF.Extensions;

public static class HealthCheckServiceCollectionExtensions
{
    public static IHealthChecksBuilder AddSqliteHealthCheck(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration)
        => healthChecksBuilder.AddSqlite(configuration.GetConnectionString("Sqlite"), name: "SQLite", tags: ["database", "storage", "sql"]);
}