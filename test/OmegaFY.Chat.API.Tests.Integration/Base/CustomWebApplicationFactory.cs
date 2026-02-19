using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Data.EF.Context;
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
        builder.UseEnvironment(Environments.Staging);
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
        Console.WriteLine(">>> Iniciando ResetDatabaseAsync...");

        using IServiceScope scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // 1. Log da Connection String original
        var originalConn = config.GetConnectionString("Sqlite");
        Console.WriteLine($">>> Connection String Original: {originalConn}");

        var builder = new SqliteConnectionStringBuilder(originalConn);
        var fileName = Path.GetFileName(builder.DataSource);

        // 2. Log do diretório temporário e novo caminho
        var tempPath = Path.GetTempPath();
        var absoluteDbPath = Path.Combine(tempPath, fileName);
        Console.WriteLine($">>> Path Temporário do SO: {tempPath}");
        Console.WriteLine($">>> Caminho Absoluto Final: {absoluteDbPath}");

        // 3. Aplica a nova connection string ao contexto
        builder.DataSource = absoluteDbPath;
        context.Database.GetDbConnection().ConnectionString = builder.ConnectionString;
        Console.WriteLine($">>> Connection String Atualizada: {context.Database.GetDbConnection().ConnectionString}");

        // 4. Operações de Banco
        Console.WriteLine(">>> Limpando pools de conexão...");
        SqliteConnection.ClearAllPools();

        Console.WriteLine(">>> Executando EnsureDeletedAsync...");
        await context.Database.EnsureDeletedAsync();

        Console.WriteLine(">>> Executando MigrateAsync...");
        await context.Database.MigrateAsync();

        Console.WriteLine(">>> ResetDatabaseAsync concluído com sucesso!");
    }
}