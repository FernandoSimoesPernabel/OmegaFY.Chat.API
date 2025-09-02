using OmegaFY.Chat.API.Infra.Constants;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Application.Shared.Extensions;

public static class ActivityExtensions
{
    public static Activity SetHandlerName(this Activity activity, string handlerName) 
        => activity.SetTag(OpenTelemetryConstants.HANDLER_NAME_KEY, handlerName);

    public static Activity SetRequest(this Activity activity, IRequest request) 
        => activity.SetTag(OpenTelemetryConstants.REQUEST_CONTENT_KEY, request?.ToString());

    public static Activity SetResult(this Activity activity, HandlerResult result)
    {
        activity.SetTag(OpenTelemetryConstants.RESULT_CONTENT_KEY, result?.ToString());

        return activity.SetStatus(result);
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