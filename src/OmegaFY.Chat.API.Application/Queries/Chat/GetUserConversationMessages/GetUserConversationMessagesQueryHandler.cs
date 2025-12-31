using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed class GetUserConversationMessagesQueryHandler : QueryHandlerBase<GetUserConversationMessagesQueryHandler, GetUserConversationMessagesQuery, GetUserConversationMessagesQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IUserInformation _userInformation;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    public GetUserConversationMessagesQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserConversationMessagesQuery> validator,
        ILogger<GetUserConversationMessagesQueryHandler> logger,
        IChatQueryProvider chatQueryProvider,
        IUserInformation userInformation,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
        _userInformation = userInformation;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<GetUserConversationMessagesQueryResult>> InternalHandleAsync(GetUserConversationMessagesQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetUserConversationMessagesQueryResult>();

        Guid userId = _userInformation.CurrentRequestUserId.Value;

        (_, (MessageFromMemberModel[] messageFromMembers, PaginationResultInfo paginationInfo) result) =
            await _hybridCacheProvider.GetOrCreateAsync(
                CacheKeyGenerator.UserConversationMessagesKey(request.ConversationId, userId, request.Pagination.PageNumber, request.Pagination.PageSize),
                async (cancellationToken) => await _chatQueryProvider.GetMessagesFromMemberAsync(request.ConversationId, userId, request.Pagination, cancellationToken),
                new CacheOptions()
                {
                    Expiration = TimeSpanConstants.TEN_MINUTES,
                    LocalCacheExpiration = TimeSpanConstants.TEN_MINUTES,
                    Tags = ["chat", "chat:messages", $"chat:conversation:{request.ConversationId}", $"chat:user:{userId}", $"user:{userId}"]
                },
                cancellationToken);

        return HandlerResult.Create(new GetUserConversationMessagesQueryResult(result.messageFromMembers, result.paginationInfo));
    }
}