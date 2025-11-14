using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;

public sealed class GetMemberFromConversationQueryHandler : QueryHandlerBase<GetMemberFromConversationQueryHandler, GetMemberFromConversationQuery, GetMemberFromConversationQueryResult>
{
    public GetMemberFromConversationQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMemberFromConversationQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
    }

    protected override Task<HandlerResult<GetMemberFromConversationQueryResult>> InternalHandleAsync(GetMemberFromConversationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}