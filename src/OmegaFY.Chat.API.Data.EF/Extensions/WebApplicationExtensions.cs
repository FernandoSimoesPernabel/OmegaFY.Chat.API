using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Data.EF.Context;

namespace OmegaFY.Chat.API.Data.EF.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunMigrationsAsync(this WebApplication app)
    {
        if (app.Environment.IsStaging())
            return;

        using IServiceScope scope = app.Services.CreateScope();
        
        ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
        await context.Database.MigrateAsync();
    }
}