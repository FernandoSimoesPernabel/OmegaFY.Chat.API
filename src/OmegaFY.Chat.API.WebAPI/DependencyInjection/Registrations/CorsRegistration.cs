using OmegaFY.Chat.API.WebAPI.Models.Configs;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class CorsRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            CorsSettings corsSettings = builder.Configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

            options.AddDefaultPolicy(policy =>
            {
                policy.WithMethods(corsSettings.AllowedMethods);

                policy.WithHeaders(corsSettings.AllowedHeaders);

                policy.WithOrigins(corsSettings.AllowedOrigins);

                policy.AllowCredentials();
            });
        });
    }
}