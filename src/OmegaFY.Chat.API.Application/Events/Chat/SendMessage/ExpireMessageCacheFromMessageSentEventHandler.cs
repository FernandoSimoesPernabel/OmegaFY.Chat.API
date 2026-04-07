using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

internal sealed class ExpireMessageCacheFromMessageSentEventHandler : EventHandlerHandlerBase<MessageSentEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    private readonly IConversationRepository _conversationRepository;

    public ExpireMessageCacheFromMessageSentEventHandler(IHybridCacheProvider hybridCacheProvider, IConversationRepository conversationRepository)
    {
        _hybridCacheProvider = hybridCacheProvider;
        _conversationRepository = conversationRepository;
    }

    protected async override Task HandleAsync(MessageSentEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatMessageIdTag(@event.MessageId), cancellationToken);

        Conversation conversation = await _conversationRepository.GetConversationByIdAsync(@event.ConversationId, cancellationToken);

        foreach (Guid memberUserId in conversation.GetMembersUserIds())
            await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(memberUserId), cancellationToken);
    }
}