namespace OmegaFY.Chat.API.Application.Events.Users.RemoveFriendship;

public sealed record class FriendshipRemovedEvent : IEvent
{
    public Guid FriendshipId { get; init; }

    public Guid RequestingUserId { get; init; }

    public Guid InvitedUserId { get; init; }

    public DateTime RemovedDate { get; init; }

    public FriendshipRemovedEvent() { }

    public FriendshipRemovedEvent(Guid friendshipId, Guid requestingUserId, Guid invitedUserId)
    {
        FriendshipId = friendshipId;
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        RemovedDate = DateTime.UtcNow;
    }
}