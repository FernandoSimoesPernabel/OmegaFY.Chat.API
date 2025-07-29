using System.Diagnostics;

namespace OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

public interface IOpenTelemetryRegisterProvider
{
    public Activity StartActivity(string activityName);
}