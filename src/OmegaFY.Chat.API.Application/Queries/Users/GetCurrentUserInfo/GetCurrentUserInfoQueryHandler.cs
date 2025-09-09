using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

public sealed class GetCurrentUserInfoQueryHandler : QueryHandlerBase<GetCurrentUserInfoQueryHandler, GetCurrentUserInfoQuery, GetCurrentUserInfoQueryResult>
{
    public GetCurrentUserInfoQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetCurrentUserInfoQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator) { }

    protected override Task<HandlerResult<GetCurrentUserInfoQueryResult>> InternalHandleAsync(GetCurrentUserInfoQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}