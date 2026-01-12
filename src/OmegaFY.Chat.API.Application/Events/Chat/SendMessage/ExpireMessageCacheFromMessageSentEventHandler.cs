using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

internal sealed class ExpireMessageCacheFromMessageSentEventHandler : EventHandlerHandlerBase<MessageSentEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireMessageCacheFromMessageSentEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(MessageSentEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatMessageIdTag(@event.MessageId), cancellationToken);
    }
}