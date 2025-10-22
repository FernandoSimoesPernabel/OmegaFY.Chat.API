namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

public sealed record class FriendshipAcceptedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public DateTime AcceptedDate { get; init; }

    public FriendshipAcceptedEvent() { }

    public FriendshipAcceptedEvent(Guid friendshipId)
    {
        FriendshipId = friendshipId;
        AcceptedDate = DateTime.UtcNow;
    }
}