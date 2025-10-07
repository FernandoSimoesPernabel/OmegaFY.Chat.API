using OmegaFY.Chat.API.Domain.Enums;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class FriendshipModel
{
    public Guid FriendshipId { get; init; }

    public Guid RequestingUserId { get; init; }

    public Guid InvitedUserId { get; init; }

    public DateTime StartedDate { get; init; }

    public FriendshipStatus Status { get; init; }

    public FriendshipModel() { }

    public FriendshipModel(Guid friendshipId, Guid requestingUserId, Guid invitedUserId, DateTime startedDate, FriendshipStatus status)
    {
        FriendshipId = friendshipId;
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        StartedDate = startedDate;
        Status = status;
    }
}