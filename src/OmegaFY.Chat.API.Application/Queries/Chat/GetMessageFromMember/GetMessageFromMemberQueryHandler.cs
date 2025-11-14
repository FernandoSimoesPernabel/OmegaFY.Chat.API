using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed class GetMessageFromMemberQueryHandler : QueryHandlerBase<GetMessageFromMemberQueryHandler, GetMessageFromMemberQuery, GetMessageFromMemberQueryResult>
{
    public GetMessageFromMemberQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMessageFromMemberQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
    }

    protected override Task<HandlerResult<GetMessageFromMemberQueryResult>> InternalHandleAsync(GetMessageFromMemberQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}