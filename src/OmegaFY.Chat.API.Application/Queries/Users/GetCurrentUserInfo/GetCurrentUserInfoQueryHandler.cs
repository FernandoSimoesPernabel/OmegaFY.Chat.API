using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

public sealed class GetCurrentUserInfoQueryHandler : QueryHandlerBase<GetCurrentUserInfoQueryHandler, GetCurrentUserInfoQuery, GetCurrentUserInfoQueryResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserQueryProvider _userQueryProvider;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    public GetCurrentUserInfoQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetCurrentUserInfoQuery> validator,
        ILogger<GetCurrentUserInfoQueryHandler> logger,
        IUserInformation userInformation,
        IUserQueryProvider userQueryProvider,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _userInformation = userInformation;
        _userQueryProvider = userQueryProvider;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<GetCurrentUserInfoQueryResult>> InternalHandleAsync(GetCurrentUserInfoQuery query, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetCurrentUserInfoQueryResult>();

        Guid userId = _userInformation.CurrentRequestUserId.Value;

        (_, GetCurrentUserInfoQueryResult result) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.CurrentUserInfoKey(userId),
            async (cancellationToken) => await _userQueryProvider.GetCurrentUserInfoAsync(userId, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.SEVEN_DAYS,
                LocalCacheExpiration = TimeSpanConstants.SEVEN_DAYS,
                Tags = ["users", "users:current", $"users:user:{userId}", $"user:{userId}"]
            },
            cancellationToken);

        if (result is null)
            return HandlerResult.CreateNotFound<GetCurrentUserInfoQueryResult>();

        return HandlerResult.Create(result);
    }
}