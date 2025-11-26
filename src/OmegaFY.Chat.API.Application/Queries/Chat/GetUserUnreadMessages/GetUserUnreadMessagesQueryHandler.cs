using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;

public sealed class GetUserUnreadMessagesQueryHandler : QueryHandlerBase<GetUserUnreadMessagesQueryHandler, GetUserUnreadMessagesQuery, GetUserUnreadMessagesQueryResult>
{
    public GetUserUnreadMessagesQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserUnreadMessagesQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    { }

    protected override Task<HandlerResult<GetUserUnreadMessagesQueryResult>> InternalHandleAsync(GetUserUnreadMessagesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}