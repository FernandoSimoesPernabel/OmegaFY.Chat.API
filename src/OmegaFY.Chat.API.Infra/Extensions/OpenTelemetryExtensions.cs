using OmegaFY.Chat.API.Infra.Constants;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class OpenTelemetryExtensions
{
    public static bool ShouldMonitorRoute(this string route) => route?.Contains(OpenTelemetryConstants.API_ROUTE) ?? false;
}