using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUsers;

public sealed class GetUsersQueryHandler : QueryHandlerBase<GetUsersQueryHandler, GetUsersQuery, GetUsersQueryResult>
{
    public GetUsersQueryHandler(IHostEnvironment hostEnvironment, IOpenTelemetryRegisterProvider openTelemetryRegisterProvider, IValidator<GetUsersQuery> validator)
        : base(hostEnvironment, openTelemetryRegisterProvider, validator) { }

    protected override Task<HandlerResult<GetUsersQueryResult>> InternalHandleAsync(GetUsersQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}