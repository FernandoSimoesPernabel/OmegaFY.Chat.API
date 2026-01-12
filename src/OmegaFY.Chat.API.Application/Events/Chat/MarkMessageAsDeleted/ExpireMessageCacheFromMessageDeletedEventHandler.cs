using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.MarkMessageAsDeleted;

internal sealed class ExpireMessageCacheFromMessageDeletedEventHandler : EventHandlerHandlerBase<MessageDeletedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireMessageCacheFromMessageDeletedEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(MessageDeletedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatMessageIdTag(@event.MessageId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(@event.UserId), cancellationToken);
    }
}