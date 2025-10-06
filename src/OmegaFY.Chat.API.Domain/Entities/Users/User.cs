using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Domain.Entities.Users;

public sealed class User : Entity, IAggregateRoot<User>
{
    private readonly List<Friend> _friendsRequested = new();

    private readonly List<Friend> _friendsAccepted = new();

    public string Email { get; }

    public string DisplayName { get; private set; }

    public IReadOnlyCollection<Friend> Friends => _friendsRequested.Concat(_friendsAccepted).ToArray();

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

    public void SendFriendshipRequest(ReferenceId invitedUserId)
    {
        if (invitedUserId == Id)
            throw new DomainArgumentException("Um usuário não pode enviar uma solicitação de amizade para si mesmo.");

        if (Friends.Any(friend =>
            (friend.RequestingUserId == Id && friend.InvitedUserId == invitedUserId) ||
            (friend.RequestingUserId == invitedUserId && friend.InvitedUserId == Id)))
        {
            throw new DomainArgumentException("Já existe uma solicitação de amizade entre esses usuários.");
        }

        _friendsRequested.Add(new Friend(Id, invitedUserId));
    }

    public void AcceptFriendshipRequest(ReferenceId friendId) => GetFriendIfPending(friendId).Accept();

    public void RejectFriendshipRequest(ReferenceId friendId) => GetFriendIfPending(friendId).Reject();

    public void RemoveFriendship(ReferenceId friendId)
    {
        if (_friendsRequested.RemoveAll(friend => friend.Id == friendId) > 0)
            _friendsAccepted.RemoveAll(friend => friend.Id == friendId);
    }

    private Friend GetFriendIfPending(ReferenceId friendId)
    {
        Friend friend = Friends.FirstOrDefault(friend => friend.Id == friendId);

        if (friend is null)
            throw new NotFoundException("Solicitação de amizade não encontrada.");

        if (friend.InvitedUserId != Id)
            throw new DomainInvalidOperationException("A solicitação de amizade não pertence a este usuário.");

        if (friend.Status != Enums.FriendshipStatus.Pending)
            throw new DomainInvalidOperationException("A solicitação de amizade já foi respondida.");

        return friend;
    }
}