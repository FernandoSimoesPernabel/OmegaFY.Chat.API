namespace OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;

public sealed record class FriendshipRejectedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public Guid RequestingUserId { get; init; }

    public Guid InvitedUserId { get; init; }

    public DateTime RejectionDate { get; init; }

    public FriendshipRejectedEvent() { }

    public FriendshipRejectedEvent(Guid friendshipId, Guid requestingUserId, Guid invitedUserId)
    {
        FriendshipId = friendshipId;
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        RejectionDate = DateTime.UtcNow;
    }
}