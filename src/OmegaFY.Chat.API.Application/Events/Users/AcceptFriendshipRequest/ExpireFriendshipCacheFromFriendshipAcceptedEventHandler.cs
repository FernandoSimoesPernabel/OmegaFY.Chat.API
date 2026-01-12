using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

internal sealed class ExpireFriendshipCacheFromFriendshipAcceptedEventHandler : EventHandlerHandlerBase<FriendshipAcceptedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireFriendshipCacheFromFriendshipAcceptedEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(FriendshipAcceptedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.FriendshipIdTag(@event.FriendshipId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.UsersUserIdTag(@event.RequestingUserId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.UsersUserIdTag(@event.InvitedUserId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(@event.RequestingUserId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.ChatUserIdTag(@event.InvitedUserId), cancellationToken);
    }
}