using Microsoft.Extensions.Options;
using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Configs;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Providers.Implementations;

internal sealed class OpenTelemetryActivitySourceProvider : IOpenTelemetryRegisterProvider
{
    private readonly ActivitySource _activitySource;

    public OpenTelemetryActivitySourceProvider(IOptions<OpenTelemetrySettings> openTelemetrySettings)
        => _activitySource = new ActivitySource(openTelemetrySettings.Value.ServiceName, ProjectVersion.Instance.ToString());

    public Activity ContinueParentActivity(string activityName, Dictionary<string, string> parentItems)
    {
        PropagationContext parentContext = OpenTelemetryPropagatorsHelper.GetPropagationContext(parentItems);

        Baggage.Current = parentContext.Baggage;

        Activity previousActivity = _activitySource.StartActivity(activityName, ActivityKind.Consumer, parentContext.ActivityContext);

        return previousActivity;
    }

    public Activity StartActivity(string activityName) => _activitySource.StartActivity(activityName);
}