using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.MarkMessageAsRead;

internal sealed class ExpireMessageCacheFromMessageReadEventHandler : EventHandlerHandlerBase<MessageReadEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireMessageCacheFromMessageReadEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(MessageReadEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatMessageIdTag(@event.MessageId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(@event.UserId), cancellationToken);
    }
}