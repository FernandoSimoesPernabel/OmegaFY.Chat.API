using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversations;

public sealed class GetUserConversationsQueryHandler : QueryHandlerBase<GetUserConversationsQueryHandler, GetUserConversationsQuery, GetUserConversationsQueryResult>
{
    public GetUserConversationsQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserConversationsQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    { }

    protected override Task<HandlerResult<GetUserConversationsQueryResult>> InternalHandleAsync(GetUserConversationsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}