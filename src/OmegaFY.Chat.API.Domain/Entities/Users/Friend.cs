using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Users;

public sealed class Friend : Entity
{
    public ReferenceId RequestingUserId { get; }

    public ReferenceId InvitedUserId { get; }

    public DateTime StartedDate { get; }

    public FriendshipStatus Status { get; private set; }

    public Friend(ReferenceId requestingUserId, ReferenceId invitedUserId)
    {
        RequestingUserId = requestingUserId;
        InvitedUserId = invitedUserId;
        StartedDate = DateTime.UtcNow;
        Status = FriendshipStatus.Pending;
    }

    public void Accept() => Status = FriendshipStatus.Accepted;

    public void Reject() => Status = FriendshipStatus.Rejected; 
}