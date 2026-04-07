using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.ChangeGroupConfig;

internal sealed class ExpireConversationCacheFromGroupConfigChangedEventHandler : EventHandlerHandlerBase<GroupConfigChangedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    private readonly IConversationRepository _conversationRepository;

    public ExpireConversationCacheFromGroupConfigChangedEventHandler(IHybridCacheProvider hybridCacheProvider, IConversationRepository conversationRepository)
    {
        _hybridCacheProvider = hybridCacheProvider;
        _conversationRepository = conversationRepository;
    }

    protected async override Task HandleAsync(GroupConfigChangedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);

        Conversation conversation = await _conversationRepository.GetConversationByIdAsync(@event.ConversationId, cancellationToken);

        foreach (Guid memberUserId in conversation.GetMembersUserIds())
            await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(memberUserId), cancellationToken);
    }
}