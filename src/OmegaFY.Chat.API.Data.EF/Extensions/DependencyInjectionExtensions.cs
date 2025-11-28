using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Data.EF.Authentication.Services;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Interceptors;
using OmegaFY.Chat.API.Data.EF.Repositories.Chat;
using OmegaFY.Chat.API.Data.EF.Repositories.Users;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Authentication.Services;

namespace OmegaFY.Chat.API.Data.EF.Extensions;

public static class DependencyInjectionExtensions
{
    public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder identityBuilder)
        => identityBuilder.AddEntityFrameworkStores<ApplicationContext>();

    public static IServiceCollection AddEntityFrameworkUserManager(this IServiceCollection services)
        => services.AddScoped<IUserManager, EntityFrameworkUserManager>();

    public static IServiceCollection AddSqlServerEntityFrameworkContexts(this IServiceCollection services, IConfigurationRoot configuration, IHostEnvironment environment)
    {
        string connectionString = configuration.GetConnectionString("AzureSql");

        return services.AddDbContextPool<ApplicationContext>(options =>
        {
            options.UseSqlServer(connectionString);

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();

                options.AddInterceptors(
                    new CustomDbCommandInterceptor(),
                    new CustomDbConnectionInterceptor(),
                    new CustomDbTransactionInterceptor(),
                    new CustomSaveChangesInterceptor());
            }
        });
    }

    public static IServiceCollection AddEntityFrameworkRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMemberMessageRepository, MemberMessageRepository>();

        return services;
    }
}