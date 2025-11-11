using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMember;

public sealed class GetMemberQueryHandler : QueryHandlerBase<GetMemberQueryHandler, GetMemberQuery, GetMemberQueryResult>
{
    public GetMemberQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMemberQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
    }

    protected override Task<HandlerResult<GetMemberQueryResult>> InternalHandleAsync(GetMemberQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}