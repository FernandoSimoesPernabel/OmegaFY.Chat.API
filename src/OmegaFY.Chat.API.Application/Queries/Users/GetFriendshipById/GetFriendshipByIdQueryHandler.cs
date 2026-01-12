using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;

public sealed class GetFriendshipByIdQueryHandler : QueryHandlerBase<GetFriendshipByIdQueryHandler, GetFriendshipByIdQuery, GetFriendshipByIdQueryResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserQueryProvider _userQueryProvider;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    public GetFriendshipByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetFriendshipByIdQuery> validator,
        ILogger<GetFriendshipByIdQueryHandler> logger,
        IUserInformation userInformation,
        IUserQueryProvider userQueryProvider,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _userInformation = userInformation;
        _userQueryProvider = userQueryProvider;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected async override Task<HandlerResult<GetFriendshipByIdQueryResult>> InternalHandleAsync(GetFriendshipByIdQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetFriendshipByIdQueryResult>();

        Guid userId = _userInformation.CurrentRequestUserId.Value;

        (_, FriendshipModel friendshipModel) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.FriendshipByIdKey(userId, request.FriendshipId),
            async (cancellationToken) => await _userQueryProvider.GetFriendshipByIdAndUserIdAsync(userId, request.FriendshipId, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.THIRTY_DAYS,
                LocalCacheExpiration = TimeSpanConstants.THIRTY_DAYS,
                Tags = [CacheTagsGenerator.UsersTag(), CacheTagsGenerator.UsersFriendshipsTag(), CacheTagsGenerator.UsersUserIdTag(userId), CacheTagsGenerator.UserIdTag(userId), CacheTagsGenerator.FriendshipIdTag(request.FriendshipId)]
            },
            cancellationToken);

        if (friendshipModel is null)
            return HandlerResult.CreateNotFound<GetFriendshipByIdQueryResult>();

        return HandlerResult.Create(new GetFriendshipByIdQueryResult(friendshipModel));
    }
}