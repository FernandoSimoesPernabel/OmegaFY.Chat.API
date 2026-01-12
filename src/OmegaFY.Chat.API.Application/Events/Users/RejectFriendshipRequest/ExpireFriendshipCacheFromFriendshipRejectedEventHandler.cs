using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;

namespace OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;

internal sealed class ExpireFriendshipCacheFromFriendshipRejectedEventHandler : EventHandlerHandlerBase<FriendshipRejectedEvent>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    public ExpireFriendshipCacheFromFriendshipRejectedEventHandler(IHybridCacheProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

    protected override async Task HandleAsync(FriendshipRejectedEvent @event, CancellationToken cancellationToken)
    {
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.FriendshipIdTag(@event.FriendshipId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.UsersUserIdTag(@event.RequestingUserId), cancellationToken);
        await _hybridCacheProvider.RemoveByTagAsync(CacheTagsGenerator.UsersUserIdTag(@event.InvitedUserId), cancellationToken);
    }
}