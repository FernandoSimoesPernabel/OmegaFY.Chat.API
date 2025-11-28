namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

public sealed record class FriendshipAcceptedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public Guid RequestingUserId { get; init; }

    public Guid InvitedUserId { get; init; }

    public FriendshipAcceptedEvent() { }

    public FriendshipAcceptedEvent(Guid friendshipId, Guid requestingUserId, Guid invitedUserId)
    {
        FriendshipId = friendshipId;
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
    }
}