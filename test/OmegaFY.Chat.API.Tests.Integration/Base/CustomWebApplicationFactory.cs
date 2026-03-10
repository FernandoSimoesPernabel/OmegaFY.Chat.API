using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Infra.Hubs;
using OmegaFY.Chat.API.Tests.Integration.Mocks;
using System.Reflection;

namespace OmegaFY.Chat.API.Tests.Integration.Base;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public async ValueTask InitializeAsync()
    {
        await ResetDatabaseAsync();
    }

    public new ValueTask DisposeAsync() => ValueTask.CompletedTask;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Staging);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IChatNotificationProvider>();
            services.AddSingleton<IChatNotificationProvider, MockChatNotificationProvider>();
        });
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