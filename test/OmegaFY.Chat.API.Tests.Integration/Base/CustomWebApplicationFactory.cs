using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OmegaFY.Chat.API.Tests.Integration.Base;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private string _connectionString;

    public async Task InitializeAsync()
    {
        await ResetDatabaseAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddUserSecrets<CustomWebApplicationFactory>();
            _connectionString = config.Build().GetConnectionString("AzureSql");
        });
    }

    private async Task ResetDatabaseAsync()
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using SqlCommand command = new SqlCommand("EXEC [dbo].[sp_ResetDatabaseTables]", connection);
        await command.ExecuteNonQueryAsync();
    }
}