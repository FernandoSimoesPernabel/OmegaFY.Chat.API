using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Extensions;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers.Implementations;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
                    honeycombOptions.ApiKey = openTelemetrySettings.HoneycombApiKey;
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
}