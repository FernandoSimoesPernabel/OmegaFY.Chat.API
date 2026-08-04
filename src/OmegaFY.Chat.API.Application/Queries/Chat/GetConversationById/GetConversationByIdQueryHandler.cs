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

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed class GetConversationByIdQueryHandler : QueryHandlerBase<GetConversationByIdQueryHandler, GetConversationByIdQuery, GetConversationByIdQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    private readonly IUserInformation _userInformation;

    public GetConversationByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetConversationByIdQuery> validator,
        ILogger<GetConversationByIdQueryHandler> logger,
        IChatQueryProvider chatQueryProvider,
        IHybridCacheProvider hybridCacheProvider,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
        _hybridCacheProvider = hybridCacheProvider;
        _userInformation = userInformation;
    }

    protected async override Task<HandlerResult<GetConversationByIdQueryResult>> InternalHandleAsync(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<GetConversationByIdQueryResult>();

        (_, ConversationAndMembersModel conversation) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.ConversationByIdKey(request.ConversationId, _userInformation.CurrentRequestUserId.Value),
            async (cancellationToken) => await _chatQueryProvider.GetConversationByIdAsync(request.ConversationId, _userInformation.CurrentRequestUserId.Value, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.TWELVE_HOURS,
                LocalCacheExpiration = TimeSpanConstants.TWELVE_HOURS,
                Tags =
                [
                    CacheTagsGenerator.ChatTag(),
                    CacheTagsGenerator.ChatConversationsTag(),
                    CacheTagsGenerator.ChatConversationIdTag(request.ConversationId),
                    CacheTagsGenerator.ChatUserIdTag(_userInformation.CurrentRequestUserId.Value)
                ]
            },
            cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<GetConversationByIdQueryResult>();

        return HandlerResult.Create(new GetConversationByIdQueryResult(conversation));
    }
}