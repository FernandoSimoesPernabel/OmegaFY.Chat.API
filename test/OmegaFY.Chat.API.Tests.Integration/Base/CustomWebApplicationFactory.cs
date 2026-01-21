using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace OmegaFY.Chat.API.Tests.Integration.Base;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await ResetDatabaseAsync();
    }

    public new Task DisposeAsync() => Task.CompletedTask;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddUserSecrets(Assembly.GetAssembly(typeof(Program)), true);
            config.AddUserSecrets<CustomWebApplicationFactory>(true);
        });

        return base.CreateHost(builder);
    }

    private async Task ResetDatabaseAsync()
    {
        await using SqlConnection connection = new SqlConnection(Services.GetRequiredService<IConfiguration>().GetConnectionString("AzureSql"));
        
        await connection.OpenAsync();

        await using SqlCommand command = new("[dbo].[sp_ResetDatabaseTables]", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        await command.ExecuteNonQueryAsync();
    }
}