using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Diagnostics;
using System.Linq;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class ActivityExtensions
{
    public static Activity SetHandlerName(this Activity activity, string handlerName)
        => activity.SetTag(OpenTelemetryConstants.HANDLER_NAME_KEY, handlerName);

    public static Activity SetMessage(this Activity activity, MessageEnvelope message)
    {
        activity.SetTag(OpenTelemetryConstants.MESSAGE_ID_KEY, message.Id.ToString());
        activity.SetTag(OpenTelemetryConstants.MESSAGE_PAYLOAD_KEY, message.Payload.ToString());

        return activity;
    }

    public static Activity SetCacheKey(this Activity activity, string cacheKey) => activity.SetTag(OpenTelemetryConstants.CACHE_KEY, cacheKey);

    public static Activity SetCacheHit(this Activity activity, bool cacheHit) => activity.SetTag(OpenTelemetryConstants.CACHE_HIT, cacheHit);

    public static Activity SetCacheTags(this Activity activity, string[] tags) => activity.SetTag(OpenTelemetryConstants.CACHE_TAGS, string.Join(';', tags ?? []));

    public static Activity SetOkStatus(this Activity activity) => activity.SetStatus(ActivityStatusCode.Ok);

    public static Activity SetErrorStatus(this Activity activity) => activity.SetStatus(ActivityStatusCode.Error);

    public static Activity SetErrorStatus(this Activity activity, string description) => activity.SetStatus(ActivityStatusCode.Error, description);

    public static Activity SetErrorStatus(this Activity activity, Exception ex)
    {
        activity.AddException(ex);
        return activity.SetStatus(ActivityStatusCode.Error, ex.Message);
    }
}