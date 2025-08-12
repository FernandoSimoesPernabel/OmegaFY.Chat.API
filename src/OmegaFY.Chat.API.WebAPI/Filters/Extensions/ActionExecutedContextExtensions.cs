using Microsoft.AspNetCore.Mvc.Filters;
using OmegaFY.Chat.API.Application.Shared;

namespace OmegaFY.Chat.API.WebAPI.Filters.Extensions;

public static class ActionExecutedContextExtensions
{
    public static HandlerResult ToHandlerResult(this ActionExecutedContext context) => (context.Result as ObjectResult)?.Value as HandlerResult;
}