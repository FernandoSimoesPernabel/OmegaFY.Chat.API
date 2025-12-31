using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class ActivityExtensions
{
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
            return activity.SetErrorStatus();

        if (result.Succeeded())
            return activity.SetOkStatus();

        return activity.SetErrorStatus(result.GetErrorsAsStringSeparatedByNewLine());
    }
}