using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Chat.AddMemberToGroup;

internal sealed class ExpireConversationCacheFromMemberAddedToGroupEventHandler : EventHandlerHandlerBase<MemberAddedToGroupEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireConversationCacheFromMemberAddedToGroupEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(MemberAddedToGroupEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatConversationIdTag(@event.ConversationId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(@event.UserId), cancellationToken);
    }
}