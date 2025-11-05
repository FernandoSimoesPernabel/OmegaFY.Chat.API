using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Data.EF.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class HealthCheckRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks().AddSqlServerHealthCheck(builder.Configuration);

        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(600);
            options.SetMinimumSecondsBetweenFailureNotifications(30);

            options.AddHealthCheckEndpoint(ApplicationInfoConstants.APPLICATION_NAME, HealthCheckConstants.API_ENDPOINT);
        }).AddInMemoryStorage();
    }
}
