using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Infra.Authentication.JwtEvents;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;
using OmegaFY.Chat.API.Infra.Authentication.Services.Implementations;
using OmegaFY.Chat.API.Infra.Authentication.Users;
using OmegaFY.Chat.API.Infra.Authentication.Users.Implementations;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Implementations;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers.Implementations;
using OmegaFY.Chat.API.Infra.RateLimite.Configs;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.RateLimiting;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        OpenTelemetrySettings openTelemetrySettings = configuration.GetSection(nameof(OpenTelemetrySettings)).Get<OpenTelemetrySettings>();

        services.Configure<OpenTelemetrySettings>(configuration.GetSection(nameof(OpenTelemetrySettings)));

        services.AddSingleton<IOpenTelemetryRegisterProvider, OpenTelemetryActivitySourceProvider>();

        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(openTelemetrySettings.ServiceName, serviceVersion: ProjectVersion.Instance.ToString());

        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder.AddSource(openTelemetrySettings.ServiceName).SetResourceBuilder(resourceBuilder)
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
            }).WithMetrics(builder =>
            {
                builder.AddHttpClientInstrumentation().SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHoneycomb(honeycombOptions =>
                    {
                        honeycombOptions.ServiceName = openTelemetrySettings.ServiceName;
                        honeycombOptions.ApiKey = openTelemetrySettings.HoneycombSettings.HoneycombApiKey;
                        honeycombOptions.ServiceVersion = ProjectVersion.Instance.ToString();
                    });
            }).WithLogging(builder =>
            {
                builder.SetResourceBuilder(resourceBuilder).AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = openTelemetrySettings.HoneycombSettings.HoneycombApiEndpoint;
                    otlpOptions.Headers = openTelemetrySettings.HoneycombSettings.HoneycombApiKeyHeader;
                });
            });

        return services;
    }

    public static IServiceCollection AddConcurrentBagInMemoryMessageBus(this IServiceCollection services)
        => services.AddSingleton<IMessageBus, ConcurrentBagInMemoryMessageBus>();

    public static IServiceCollection AddChannelInMemoryMessageBus(this IServiceCollection services)
        => services.AddSingleton<IMessageBus, ChannelInMemoryMessageBus>();

    public static IServiceCollection AddIdentityUserConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        services.AddScoped<CustomJwtBearerEvents>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserInformation, HttpContextAccessorUserInformation>();
        services.AddScoped<IAuthenticationService, IdentityAuthenticationService>();
        services.AddScoped<IJwtProvider, JwtSecurityTokenProvider>();

        services.AddAuthenticationSettings(configuration);

        JwtSettings jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        var tokenValidationParameters = new TokenValidationParameters()
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

    public static IServiceCollection AddAuthenticationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));

        AuthenticationSettings authSettings = configuration.GetSection(nameof(AuthenticationSettings)).Get<AuthenticationSettings>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = authSettings.PasswordRequireDigit;
            options.Password.RequireLowercase = authSettings.PasswordRequireLowercase;
            options.Password.RequireNonAlphanumeric = authSettings.PasswordRequireNonAlphanumeric;
            options.Password.RequireUppercase = authSettings.PasswordRequireUppercase;
            options.Password.RequiredLength = authSettings.PasswordMinRequiredLength;
            options.Password.RequiredUniqueChars = authSettings.PasswordRequiredUniqueChars;

            options.Lockout.DefaultLockoutTimeSpan = authSettings.DefaultLockoutTimeSpan;
            options.Lockout.MaxFailedAccessAttempts = authSettings.MaxFailedAccessAttempts;

            options.User.RequireUniqueEmail = authSettings.RequireUniqueEmail;

            options.SignIn.RequireConfirmedEmail = authSettings.RequireConfirmedEmail;
            options.SignIn.RequireConfirmedAccount = authSettings.RequireConfirmedAccount;
        });

        return services;
    }

    public static IdentityBuilder AddIdentity(this IServiceCollection services) => services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>();

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddHybridCache();

        return services;
    }

    public static IServiceCollection AddWebApiRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        IpOrUserTokenBucketPolicySettings ipOrUserTokenPolicySettings = configuration.GetSection(nameof(IpOrUserTokenBucketPolicySettings)).Get<IpOrUserTokenBucketPolicySettings>();

        services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                TokenBucketRateLimiterOptions tokenBucketOptions = new TokenBucketRateLimiterOptions()
                {
                    AutoReplenishment = true,
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    ReplenishmentPeriod = ipOrUserTokenPolicySettings.ReplenishmentPeriod,
                    TokenLimit = ipOrUserTokenPolicySettings.TokenLimit,
                    TokensPerPeriod = ipOrUserTokenPolicySettings.TokensPerPeriod
                };

                string userEmail = null;

                if (context.User.IsAuthenticated())
                    userEmail = context.User.TryGetEmailFromClaims();

                return userEmail is not null
                    ? RateLimitPartition.GetTokenBucketLimiter(userEmail, _ => tokenBucketOptions)
                    : RateLimitPartition.GetTokenBucketLimiter(context.Connection.RemoteIpAddress.ToString(), _ => tokenBucketOptions);
            });

            limiterOptions.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
                    context.HttpContext.Response.Headers.RetryAfter = retryAfter.TotalSeconds.ToString();

                await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later...", cancellationToken);
            };
        });

        return services;
    }
}