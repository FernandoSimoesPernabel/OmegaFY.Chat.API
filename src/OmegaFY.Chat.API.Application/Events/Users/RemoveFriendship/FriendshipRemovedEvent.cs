namespace OmegaFY.Chat.API.Application.Events.Users.RemoveFriendship;

public sealed record class FriendshipRemovedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public DateTime RemovedDate { get; init; }

    public FriendshipRemovedEvent() { }

    public FriendshipRemovedEvent(Guid friendshipId)
    {
        FriendshipId = friendshipId;
        RemovedDate = DateTime.UtcNow;
    }
}