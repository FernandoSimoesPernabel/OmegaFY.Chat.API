using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.CreateGroupConversation;

internal sealed class ExpireConversationCacheFromGroupConversationCreatedEventHandler : EventHandlerHandlerBase<GroupConversationCreatedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireConversationCacheFromGroupConversationCreatedEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(GroupConversationCreatedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(@event.CreatedByUserId), cancellationToken);
    }
}