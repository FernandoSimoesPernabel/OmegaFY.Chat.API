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

    public GetConversationByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetConversationByIdQuery> validator,
        ILogger<GetConversationByIdQueryHandler> logger,
        IChatQueryProvider chatQueryProvider,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<GetConversationByIdQueryResult>> InternalHandleAsync(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        (_, ConversationAndMembersModel conversation) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.ConversationByIdKey(request.ConversationId),
            async (cancellationToken) => await _chatQueryProvider.GetConversationByIdAsync(request.ConversationId, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.TWELVE_HOURS,
                LocalCacheExpiration = TimeSpanConstants.TWELVE_HOURS,
                Tags = ["chat", "chat:conversations", $"chat:conversation:{request.ConversationId}"]
            },
            cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<GetConversationByIdQueryResult>();

        return HandlerResult.Create(new GetConversationByIdQueryResult(conversation));
    }
}