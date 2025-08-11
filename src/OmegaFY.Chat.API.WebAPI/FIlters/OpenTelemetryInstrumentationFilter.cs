using Microsoft.AspNetCore.Mvc.Filters;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Constants;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

namespace OmegaFY.Chat.API.WebAPI.FIlters;

public sealed class OpenTelemetryInstrumentationFilter : IAsyncActionFilter
{
    private readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    public OpenTelemetryInstrumentationFilter(IOpenTelemetryRegisterProvider openTelemetryRegisterProvider)
        => _openTelemetryRegisterProvider = openTelemetryRegisterProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_APPLICATION_HANDLER_NAME);

        ActionExecutedContext resultContext = await next();

        IRequest request = context.ActionArguments.Values.OfType<IRequest>().FirstOrDefault();

        object result = (resultContext.Result as ObjectResult)?.Value;

        //if (request is not null)
        //{
        //    activity.SetCurrentRequestTracingInformation(request, result);
        //}
    }
}