using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

public sealed class GetCurrentUserInfoQueryHandler : QueryHandlerBase<GetCurrentUserInfoQueryHandler, GetCurrentUserInfoQuery, GetCurrentUserInfoQueryResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserQueryProvider _userQueryProvider;

    public GetCurrentUserInfoQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetCurrentUserInfoQuery> validator,
        IUserInformation userInformation,
        IUserQueryProvider userQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _userInformation = userInformation;
        _userQueryProvider = userQueryProvider;
    }

    protected override async Task<HandlerResult<GetCurrentUserInfoQueryResult>> InternalHandleAsync(GetCurrentUserInfoQuery query, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            HandlerResult.CreateUnauthorized<GetCurrentUserInfoQueryResult>();

        GetCurrentUserInfoQueryResult result = 
            await _userQueryProvider.GetCurrentUserInfoAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (result is null)
            HandlerResult.CreateNotFound<GetCurrentUserInfoQueryResult>();

        return HandlerResult.Create(result);
    }
}