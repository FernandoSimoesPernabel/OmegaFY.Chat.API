using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Application.Models;

public sealed record class FriendshipModel
{
    public Guid FriendshipId { get; init; }

    public ReferenceId RequestingUserId { get; init; }

    public ReferenceId InvitedUserId { get; init; }

    public DateTime StartedDate { get; init; }

    public FriendshipStatus Status { get; init; }

    public FriendshipModel() { }

    public FriendshipModel(Guid friendshipId, ReferenceId requestingUserId, ReferenceId invitedUserId, DateTime startedDate, FriendshipStatus status)
    {
        FriendshipId = friendshipId;
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        StartedDate = startedDate;
        Status = status;
    }
}