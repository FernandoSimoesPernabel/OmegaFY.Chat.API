using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Common.Helpers;

public static class OpenTelemetryPropagatorsHelper
{
    public static Dictionary<string, string> GetInjectedItems()
    {
        Dictionary<string, string> items = new Dictionary<string, string>();

        Propagators.DefaultTextMapPropagator.Inject(
            new PropagationContext(Activity.Current.Context, Baggage.Current),
            items,
            (props, key, value) => props[key] = value);

        return items;
    }
}