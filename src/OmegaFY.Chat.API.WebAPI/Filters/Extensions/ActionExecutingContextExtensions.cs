using Microsoft.AspNetCore.Mvc.Filters;
using OmegaFY.Chat.API.Application.Shared;

namespace OmegaFY.Chat.API.WebAPI.Filters.Extensions;

public static class ActionExecutingContextExtensions
{
    public static IRequest GetRequestFromContext(this ActionExecutingContext context) => context.ActionArguments.Values.OfType<IRequest>().FirstOrDefault();
}