using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversations;

public sealed class GetUserConversationsQueryHandler : QueryHandlerBase<GetUserConversationsQueryHandler, GetUserConversationsQuery, GetUserConversationsQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IUserInformation _userInformation;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    public GetUserConversationsQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserConversationsQuery> validator,
        ILogger<GetUserConversationsQueryHandler> logger,
        IChatQueryProvider chatQueryProvider,
        IUserInformation userInformation,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
        _userInformation = userInformation;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<GetUserConversationsQueryResult>> InternalHandleAsync(GetUserConversationsQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetUserConversationsQueryResult>();

        Guid userId = _userInformation.CurrentRequestUserId.Value;

        (_, UserConversationModel[] userConversations) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.UserConversationsKey(userId),
            async (cancellationToken) => await _chatQueryProvider.GetUserConversationsAsync(userId, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.ONE_HOUR,
                LocalCacheExpiration = TimeSpanConstants.ONE_HOUR,
                Tags = [CacheTagsGenerator.ChatTag(), CacheTagsGenerator.ChatConversationsTag(), CacheTagsGenerator.ChatUserIdTag(userId), CacheTagsGenerator.UserIdTag(userId)]
            },
            cancellationToken);

        return HandlerResult.Create(new GetUserConversationsQueryResult(userConversations));
    }
}