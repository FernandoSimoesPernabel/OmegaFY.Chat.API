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
            // Carrega User Secrets do WebAPI (JwtSettings, AuthenticationSettings, etc.)
            config.AddUserSecrets(Assembly.GetAssembly(typeof(Program)), true);

            // Carrega User Secrets dos testes (sobrescreve connection string)
            config.AddUserSecrets<CustomWebApplicationFactory>(true);
        });

        return base.CreateHost(builder);
    }

    private async Task ResetDatabaseAsync()
    {
        await using SqlConnection connection = new SqlConnection(Services.GetRequiredService<IConfiguration>().GetConnectionString("AzureSql"));
        
        await connection.OpenAsync();

        await using SqlCommand command = new SqlCommand("EXEC [dbo].[sp_ResetDatabaseTables]", connection);
        
        await command.ExecuteNonQueryAsync();
    }
}