using Microsoft.AspNetCore.Mvc.Filters;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Common.Extensions;

namespace OmegaFY.Chat.API.WebAPI.Filters.Extensions;

public static class ActionExecutedContextExtensions
{
    public static GenericResult ToGenericResult(this ActionExecutedContext context)
    {
        if (context.Exception is not null)
            return new GenericResult(context.Exception.GetErrorCode(), context.Exception.Message);

        return (context.Result as ObjectResult)?.Value as GenericResult;
    }
}