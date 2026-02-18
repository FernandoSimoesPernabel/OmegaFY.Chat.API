using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
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
        await using SqliteConnection connection = new SqliteConnection(Services.GetRequiredService<IConfiguration>().GetConnectionString("Sqlite"));

        await connection.OpenAsync();

        string script = await File.ReadAllTextAsync("Scripts/000 - Clear_All_Tables.sql");

        await using SqliteCommand command = new SqliteCommand(script, connection);

        await command.ExecuteNonQueryAsync();
    }
}