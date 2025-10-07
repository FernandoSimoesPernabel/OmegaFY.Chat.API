namespace OmegaFY.Chat.API.Application.Events.Users.SendFriendshipRequest;

public sealed record class FriendshipRequestedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public Guid RequestingUserId { get; init; }

    public Guid InvitedUserId { get; init; }

    public DateTime StartedDate { get; init; }

    public FriendshipRequestedEvent() { }

    public FriendshipRequestedEvent(Guid friendshipId, Guid requestingUserId, Guid invitedUserId, DateTime startedDate)
    {
        FriendshipId = friendshipId;
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        StartedDate = startedDate;
    }
}