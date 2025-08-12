using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Infra.Authentication.Constants;
using OmegaFY.Chat.API.Infra.Authentication.JwtEvents;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;
using OmegaFY.Chat.API.Infra.Authentication.Services.Implementations;
using OmegaFY.Chat.API.Infra.Authentication.Users;
using OmegaFY.Chat.API.Infra.Authentication.Users.Implementations;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Implementations;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Extensions;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers.Implementations;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text;

namespace OmegaFY.Chat.API.Infra.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        OpenTelemetrySettings openTelemetrySettings = configuration.GetSection(nameof(OpenTelemetrySettings)).Get<OpenTelemetrySettings>();

        services.Configure<OpenTelemetrySettings>(configuration.GetSection(nameof(OpenTelemetrySettings)));

        services.AddSingleton<IOpenTelemetryRegisterProvider, OpenTelemetryActivitySourceProvider>();

        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(openTelemetrySettings.ServiceName, serviceVersion: ProjectVersion.Instance.ToString());

        services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.AddSource(openTelemetrySettings.ServiceName)
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation(aspnetOptions => aspnetOptions.Filter = (context) => context.Request.Path.Value.ShouldMonitorRoute())
                .AddHttpClientInstrumentation(httpClientOptions =>
                {
                    httpClientOptions.FilterHttpWebRequest = (context) => context.RequestUri.AbsolutePath.ShouldMonitorRoute();
                    httpClientOptions.FilterHttpRequestMessage = (context) => context.RequestUri.AbsolutePath.ShouldMonitorRoute();
                })
                .AddEntityFrameworkCoreInstrumentation(efOptions => efOptions.SetDbStatementForText = true)
                .AddHoneycomb(honeycombOptions =>
                {
                    honeycombOptions.ServiceName = openTelemetrySettings.ServiceName;
                    honeycombOptions.ApiKey = openTelemetrySettings.HoneycombSettings.HoneycombApiKey;
                    honeycombOptions.ServiceVersion = ProjectVersion.Instance.ToString();
                });
        });

        //TODO
        //services.AddLogging(loggingBuilder => loggingBuilder.AddOpenTelemetry(openTelemetryBuilder =>
        //{
        //    openTelemetryBuilder.SetResourceBuilder(resourceBuilder);
        //    openTelemetryBuilder.AddOtlpExporter(otlpOptions =>
        //    {
        //        otlpOptions.Endpoint = new Uri(openTelemetrySettings.HoneycombUrl);
        //        otlpOptions.Headers = openTelemetrySettings.HoneycombApiKeyHeader;
        //    });
        //}));

        return services;
    }

    public static IServiceCollection AddInMemoryMessageBus(this IServiceCollection services)
    {
        return services.AddSingleton<IMessageBus, InMemoryMessageBus>();
    }

    public static IServiceCollection AddIdentityUserConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        //services.AddScoped<CustomJwtBearerEvents>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserInformation, HttpContextAccessorUserInformation>();
        services.AddScoped<IAuthenticationService, IdentityAuthenticationService>();
        services.AddScoped<IJwtProvider, JwtSecurityTokenProvider>();

        JwtSettings jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            RequireAudience = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = tokenValidationParameters;
            options.EventsType = typeof(CustomJwtBearerEvents);
        });

        services.AddAuthorization(auth => auth.AddPolicy(
            PoliciesNamesConstants.BEARER_JWT_POLICY,
            new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser()
                .Build()));

        services.AddSingleton(tokenValidationParameters);

        return services;
    }
}