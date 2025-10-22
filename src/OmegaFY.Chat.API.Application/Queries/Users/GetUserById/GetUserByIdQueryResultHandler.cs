using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed class GetUserByIdQueryResultHandler : QueryHandlerBase<GetUserByIdQueryResultHandler, GetUserByIdQuery, GetUserByIdQueryResult>
{
    private readonly IUserQueryProvider _userQueryProvider;

    public GetUserByIdQueryResultHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserByIdQuery> validator,
        IUserQueryProvider userQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _userQueryProvider = userQueryProvider;
    }

    protected override async Task<HandlerResult<GetUserByIdQueryResult>> InternalHandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        GetUserByIdQueryResult result = await _userQueryProvider.GetUserByIdAsync(request.UserId, cancellationToken);

        if (result is null)
            return HandlerResult.CreateNotFound<GetUserByIdQueryResult>();

        return HandlerResult.Create(result);
    }
}