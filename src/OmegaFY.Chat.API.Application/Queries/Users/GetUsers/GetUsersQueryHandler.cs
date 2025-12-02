using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUsers;

public sealed class GetUsersQueryHandler : QueryHandlerBase<GetUsersQueryHandler, GetUsersQuery, GetUsersQueryResult>
{
    private readonly IUserQueryProvider _userQueryProvider;

    private readonly IUserInformation _userInformation;

    public GetUsersQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUsersQuery> validator,
        ILogger<GetUsersQueryHandler> logger,
        IUserQueryProvider userQueryProvider,
        IUserInformation userInformation)
        : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _userQueryProvider = userQueryProvider;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<GetUsersQueryResult>> InternalHandleAsync(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetUsersQueryResult>();

        UserModel[] users = await _userQueryProvider.GetUsersAsync(_userInformation.CurrentRequestUserId.Value, request.DisplayName, request.Status, cancellationToken);

        return HandlerResult.Create(new GetUsersQueryResult(users)); 
    }
}