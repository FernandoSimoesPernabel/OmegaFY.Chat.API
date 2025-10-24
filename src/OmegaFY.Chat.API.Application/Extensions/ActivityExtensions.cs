using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class ActivityExtensions
{
    public static Activity SetHandlerName(this Activity activity, string handlerName) 
        => activity.SetTag(OpenTelemetryConstants.HANDLER_NAME_KEY, handlerName);

    public static Activity SetRequest(this Activity activity, IRequest request) 
        => activity.SetTag(OpenTelemetryConstants.REQUEST_CONTENT_KEY, request?.ToString());

    public static Activity SetMessage(this Activity activity, MessageEnvelope message)
    {
        activity.SetTag(OpenTelemetryConstants.MESSAGE_ID_KEY, message.Id.ToString());
        activity.SetTag(OpenTelemetryConstants.MESSAGE_PAYLOAD_KEY, message.Payload.ToString());

        return activity;
    }

    public static Activity SetResult(this Activity activity, HandlerResult result)
    {
        activity.SetTag(OpenTelemetryConstants.RESULT_CONTENT_KEY, result?.ToString());

        return activity.SetStatus(result);
    }

    public static Activity SetResult(this Activity activity, Exception ex)
    {
        activity.AddException(ex);
        return activity.SetStatus(ActivityStatusCode.Error);
    }

    public static Activity SetStatus(this Activity activity, HandlerResult result)
    {
        if (result is null)
            return activity.SetStatus(ActivityStatusCode.Error);

        if (result.Succeeded())
            return activity.SetStatus(ActivityStatusCode.Ok);

        return activity.SetStatus(ActivityStatusCode.Error, result.GetErrorsAsStringSeparatedByNewLine());
    }
}