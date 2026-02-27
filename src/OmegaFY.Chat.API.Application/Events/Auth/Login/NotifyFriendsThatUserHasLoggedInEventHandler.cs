using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Auth.Login;

internal sealed class NotifyFriendsThatUserHasLoggedInEventHandler : EventHandlerHandlerBase<UserLoggedInEvent>
{
    private readonly IUserRepository _userRepository;

    private readonly IChatNotificationProvider _chatNotificationProvider;

    public NotifyFriendsThatUserHasLoggedInEventHandler(IUserRepository userRepository, IChatNotificationProvider chatNotificationProvider)
    {
        _userRepository = userRepository;
        _chatNotificationProvider = chatNotificationProvider;
    }

    protected async override Task HandleAsync(UserLoggedInEvent @event, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(@event.UserId, cancellationToken);

        if (user is null)
            return;

        await Task.WhenAll(user.Friendships.Where(friendship => friendship.IsAccepted()).Select(friendship => _chatNotificationProvider.FriendLoggedInAsync(friendship.GetFriendUserId(@event.UserId), @event.UserId)));
    }
}