using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.ChangeGroupConfig;

internal sealed class ExpireConversationCacheFromGroupConfigChangedEventHandler : EventHandlerHandlerBase<GroupConfigChangedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireConversationCacheFromGroupConfigChangedEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(GroupConfigChangedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);

        foreach (Guid memberUserId in @event.MemberUserIds)
            await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(memberUserId), cancellationToken);
    }
}