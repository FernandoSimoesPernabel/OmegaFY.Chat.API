using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OmegaFY.Chat.API.Data.EF.Extensions;

public static class HealthCheckServiceCollectionExtensions
{
    public static IHealthChecksBuilder AddSqliteHealthCheck(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration)
        => healthChecksBuilder.AddCheck("Sqlite", () =>
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(configuration.GetConnectionString("Sqlite"));
                connection.Open();
                return HealthCheckResult.Healthy("SQLite database is accessible.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("SQLite database is not accessible.", ex);
            }
        }, tags: ["database", "storage", "sql"]);
}