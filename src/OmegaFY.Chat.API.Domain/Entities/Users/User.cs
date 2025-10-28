using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Users;

public sealed class User : Entity, IAggregateRoot<User>
{
    private readonly List<Friendship> _friendshipRequested = new();

    private readonly List<Friendship> _friendshipAccepted = new();

    public string Email { get; }

    public string DisplayName { get; private set; }

    public IReadOnlyCollection<Friendship> Friendships => _friendshipRequested.Concat(_friendshipAccepted).ToArray();

    public User(string email, string displayName)
    {
        if (string.IsNullOrWhiteSpace(email) || email.Length > UserConstants.MAX_EMAIL_LENGTH)
            throw new DomainArgumentException("O Email informado esta inválido.");

        Email = email;
        ChangeDisplayName(displayName);
    }

    public void ChangeDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new DomainArgumentException("Não foi informado um nome para o usuário");

        if (displayName.Length < UserConstants.MIN_DISPLAY_NAME_LENGTH || displayName.Length > UserConstants.MAX_DISPLAY_NAME_LENGTH)
            throw new DomainArgumentException($"O nome de usuário deve ter entre {UserConstants.MIN_DISPLAY_NAME_LENGTH} e {UserConstants.MAX_DISPLAY_NAME_LENGTH}.");

        DisplayName = displayName;
    }

    public void SendFriendshipRequest(Friendship friendshipRequest)
    {
        if (friendshipRequest is null)
            throw new DomainArgumentException("A solicitação de amizade não pode ser nula.");

        if (friendshipRequest.RequestingUserId != Id)
            throw new DomainInvalidOperationException("A solicitação de amizade não pertence a este usuário."); 

        if (friendshipRequest.InvitedUserId == Id)
            throw new DomainArgumentException("Um usuário não pode enviar uma solicitação de amizade para si mesmo.");

        if (Friendships.Any(friendship =>
            (friendship.RequestingUserId == Id && friendship.InvitedUserId == friendshipRequest.InvitedUserId) ||
            (friendship.RequestingUserId == friendshipRequest.InvitedUserId && friendship.InvitedUserId == Id)))
        {
            throw new DomainArgumentException("Já existe uma solicitação de amizade entre esses usuários.");
        }

        _friendshipRequested.Add(friendshipRequest);
    }

    public void AcceptFriendshipRequest(ReferenceId friendshipId) => GetFriendshipIfPending(friendshipId).Accept();

    public void RejectFriendshipRequest(ReferenceId friendshipId) => GetFriendshipIfPending(friendshipId).Reject();

    public void RemoveFriendship(ReferenceId friendshipId)
    {
        if (_friendshipRequested.RemoveAll(friendship => friendship.Id == friendshipId) == 0)
            _friendshipAccepted.RemoveAll(friendship => friendship.Id == friendshipId);
    }

    private Friendship GetFriendshipIfPending(ReferenceId friendshipId)
    {
        Friendship friendship = Friendships.FirstOrDefault(friendship => friendship.Id == friendshipId);

        if (friendship is null)
            throw new NotFoundException("Solicitação de amizade não encontrada.");

        if (friendship.InvitedUserId != Id)
            throw new DomainInvalidOperationException("A solicitação de amizade não pertence a este usuário.");

        if (friendship.Status != Enums.FriendshipStatus.Pending)
            throw new DomainInvalidOperationException("A solicitação de amizade já foi respondida.");

        return friendship;
    }
}