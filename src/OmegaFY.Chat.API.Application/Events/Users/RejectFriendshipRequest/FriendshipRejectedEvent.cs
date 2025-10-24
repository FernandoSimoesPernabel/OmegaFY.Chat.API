namespace OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;

public sealed record class FriendshipRejectedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public DateTime RejectionDate { get; init; }

    public FriendshipRejectedEvent() { }

    public FriendshipRejectedEvent(Guid friendshipId)
    {
        FriendshipId = friendshipId;
        RejectionDate = DateTime.UtcNow;
    }
}