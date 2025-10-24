using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Users;

public sealed class Friendship : Entity
{
    public ReferenceId RequestingUserId { get; }

    public ReferenceId InvitedUserId { get; }

    public DateTime StartedDate { get; }

    public FriendshipStatus Status { get; private set; }

    public Friendship(ReferenceId requestingUserId, ReferenceId invitedUserId)
    {
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        StartedDate = DateTime.UtcNow;
        Status = FriendshipStatus.Pending;
    }

    public void Accept() => Status = FriendshipStatus.Accepted;

    public void Reject() => Status = FriendshipStatus.Rejected; 
}