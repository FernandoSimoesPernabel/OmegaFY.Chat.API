using Microsoft.Extensions.Options;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Providers.Implementations;

internal sealed class OpenTelemetryActivitySourceProvider : IOpenTelemetryRegisterProvider
{
    private readonly ActivitySource _activitySource;

    public OpenTelemetryActivitySourceProvider(IOptions<OpenTelemetrySettings> openTelemetrySettings)
        => _activitySource = new ActivitySource(openTelemetrySettings.Value.ServiceName, ProjectVersion.Instance.ToString());

    public Activity StartActivity(string activityName) => _activitySource.StartActivity(activityName);
}