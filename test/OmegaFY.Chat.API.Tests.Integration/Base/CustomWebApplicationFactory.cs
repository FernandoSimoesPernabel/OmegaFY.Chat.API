using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
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
        using IServiceScope scope = Services.CreateScope();

        ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        await context.Database.EnsureDeletedAsync();
        
        await context.Database.MigrateAsync();
    }
}