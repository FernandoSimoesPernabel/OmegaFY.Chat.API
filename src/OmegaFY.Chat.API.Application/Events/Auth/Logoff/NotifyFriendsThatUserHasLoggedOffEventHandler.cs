using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Auth.Logoff;

internal sealed class NotifyFriendsThatUserHasLoggedOffEventHandler : EventHandlerHandlerBase<UserLoggedOffEvent>
{
    private readonly IUserRepository _userRepository;

    private readonly IChatNotificationProvider _chatNotificationProvider;

    public NotifyFriendsThatUserHasLoggedOffEventHandler(IUserRepository userRepository, IChatNotificationProvider chatNotificationProvider)
    {
        _userRepository = userRepository;
        _chatNotificationProvider = chatNotificationProvider;
    }

    protected async override Task HandleAsync(UserLoggedOffEvent @event, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(@event.UserId, cancellationToken);

        if (user is null)
            return;

        await Task.WhenAll(user.Friendships.Where(friendship => friendship.IsAccepted()).Select(friendship => _chatNotificationProvider.FriendLoggedOffAsync(friendship.GetFriendUserId(@event.UserId), @event.UserId)));
    }
}