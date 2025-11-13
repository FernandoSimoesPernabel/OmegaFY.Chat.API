using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed class GetConversationByIdQueryHandler : QueryHandlerBase<GetConversationByIdQueryHandler, GetConversationByIdQuery, GetConversationByIdQueryResult>
{
    public GetConversationByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetConversationByIdQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
    }

    protected override Task<HandlerResult<GetConversationByIdQueryResult>> InternalHandleAsync(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}