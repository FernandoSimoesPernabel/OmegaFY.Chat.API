using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;
using System.Reflection;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Users;

public sealed class UserFacts
{
    [Fact]
    public void Constructor_PassingValidEmailAndDisplayName_ShouldCreateUser()
    {
        // Arrange
        string email = "test@example.com";
        string displayName = "TestUser";

        // Act
        User sut = new User(email, displayName);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(email, sut.Email);
        Assert.Equal(displayName, sut.DisplayName);
        Assert.Empty(sut.Friendships);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_PassingInvalidEmail_ShouldThrowDomainArgumentException(string invalidEmail)
    {
        // Arrange
        string displayName = "TestUser";

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new User(invalidEmail, displayName));
        Assert.Equal("O Email informado esta inválido.", exception.Message);
    }

    [Fact]
    public void Constructor_PassingEmailExceedingMaxLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        string email = new string('a', UserConstants.MAX_EMAIL_LENGTH + 1);
        string displayName = "TestUser";

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new User(email, displayName));
        Assert.Equal("O Email informado esta inválido.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_PassingInvalidDisplayName_ShouldThrowDomainArgumentException(string invalidDisplayName)
    {
        // Arrange
        string email = "test@example.com";

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new User(email, invalidDisplayName));
        Assert.Equal("Não foi informado um nome para o usuário", exception.Message);
    }

    [Fact]
    public void Constructor_PassingDisplayNameBelowMinLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        string email = "test@example.com";
        string displayName = new string('a', UserConstants.MIN_DISPLAY_NAME_LENGTH - 1);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new User(email, displayName));
        Assert.Equal($"O nome de usuário deve ter entre {UserConstants.MIN_DISPLAY_NAME_LENGTH} e {UserConstants.MAX_DISPLAY_NAME_LENGTH}.", exception.Message);
    }

    [Fact]
    public void Constructor_PassingDisplayNameExceedingMaxLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        string email = "test@example.com";
        string displayName = new string('a', UserConstants.MAX_DISPLAY_NAME_LENGTH + 1);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new User(email, displayName));
        Assert.Equal($"O nome de usuário deve ter entre {UserConstants.MIN_DISPLAY_NAME_LENGTH} e {UserConstants.MAX_DISPLAY_NAME_LENGTH}.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(GetValidDisplayNames))]
    public void Constructor_PassingValidDisplayNameLengths_ShouldCreateUser(string displayName)
    {
        // Arrange
        string email = "test@example.com";

        // Act
        User sut = new User(email, displayName);

        // Assert
        Assert.Equal(displayName, sut.DisplayName);
    }

    [Fact]
    public void ChangeDisplayName_PassingValidDisplayName_ShouldUpdateDisplayName()
    {
        // Arrange
        User sut = new User("test@example.com", "OldName");
        string newDisplayName = "NewName";

        // Act
        sut.ChangeDisplayName(newDisplayName);

        // Assert
        Assert.Equal(newDisplayName, sut.DisplayName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeDisplayName_PassingInvalidDisplayName_ShouldThrowDomainArgumentException(string invalidDisplayName)
    {
        // Arrange
        User sut = new User("test@example.com", "OldName");

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.ChangeDisplayName(invalidDisplayName));
        Assert.Equal("Não foi informado um nome para o usuário", exception.Message);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(1)]
    public void ChangeDisplayName_PassingDisplayNameBelowMinLength_ShouldThrowDomainArgumentException(int length)
    {
        // Arrange
        User sut = new User("test@example.com", "OldName");
        string displayName = new string('a', length);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.ChangeDisplayName(displayName));
        Assert.Equal($"O nome de usuário deve ter entre {UserConstants.MIN_DISPLAY_NAME_LENGTH} e {UserConstants.MAX_DISPLAY_NAME_LENGTH}.", exception.Message);
    }

    [Fact]
    public void ChangeDisplayName_PassingDisplayNameExceedingMaxLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        User sut = new User("test@example.com", "OldName");
        string displayName = new string('a', UserConstants.MAX_DISPLAY_NAME_LENGTH + 1);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.ChangeDisplayName(displayName));
        Assert.Equal($"O nome de usuário deve ter entre {UserConstants.MIN_DISPLAY_NAME_LENGTH} e {UserConstants.MAX_DISPLAY_NAME_LENGTH}.", exception.Message);
    }

    [Fact]
    public void SendFriendshipRequest_PassingValidFriendshipRequest_ShouldAddToFriendships()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship friendshipRequest = new Friendship(sut.Id, invitedUserId);

        // Act
        sut.SendFriendshipRequest(friendshipRequest);

        // Assert
        Assert.Single(sut.Friendships);
        Assert.Contains(sut.Friendships, f => f.Id == friendshipRequest.Id);
    }

    [Fact]
    public void SendFriendshipRequest_PassingNullFriendshipRequest_ShouldThrowDomainArgumentException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.SendFriendshipRequest(null));
        Assert.Equal("A solicitação de amizade não pode ser nula.", exception.Message);
    }

    [Fact]
    public void SendFriendshipRequest_PassingFriendshipRequestWithDifferentRequestingUserId_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId differentUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship friendshipRequest = new Friendship(differentUserId, invitedUserId);

        // Act & Assert
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.SendFriendshipRequest(friendshipRequest));
        Assert.Equal("A solicitação de amizade não pertence a este usuário.", exception.Message);
    }

    [Fact]
    public void SendFriendshipRequest_PassingFriendshipRequestToSelf_ShouldThrowDomainArgumentException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        Friendship friendshipRequest = new Friendship(sut.Id, sut.Id);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.SendFriendshipRequest(friendshipRequest));
        Assert.Equal("Um usuário não pode enviar uma solicitação de amizade para si mesmo.", exception.Message);
    }

    [Fact]
    public void SendFriendshipRequest_PassingDuplicateFriendshipRequest_ShouldThrowDomainArgumentException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship firstRequest = new Friendship(sut.Id, invitedUserId);
        Friendship duplicateRequest = new Friendship(sut.Id, invitedUserId);

        sut.SendFriendshipRequest(firstRequest);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.SendFriendshipRequest(duplicateRequest));
        Assert.Equal("Já existe uma solicitação de amizade entre esses usuários.", exception.Message);
    }

    [Fact]
    public void SendFriendshipRequest_PassingReverseFriendshipRequest_ShouldThrowDomainArgumentException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId otherUserId = Guid.NewGuid();
        Friendship firstRequest = new Friendship(sut.Id, otherUserId);

        sut.SendFriendshipRequest(firstRequest);

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.SendFriendshipRequest(firstRequest));
        Assert.Equal("Já existe uma solicitação de amizade entre esses usuários.", exception.Message);
    }

    [Fact]
    public void AcceptFriendshipRequest_PassingValidPendingFriendship_ShouldAcceptFriendship()
    {
        // Arrange
        User requester = new User("requester@example.com", "Requester");
        User invited = new User("invited@example.com", "Invited");

        Friendship friendshipRequest = new Friendship(requester.Id, invited.Id);
        requester.SendFriendshipRequest(friendshipRequest);
        AddFriendshipToAcceptedList(invited, friendshipRequest);

        // Act
        invited.AcceptFriendshipRequest(friendshipRequest.Id);

        // Assert
        Assert.Equal(FriendshipStatus.Accepted, friendshipRequest.Status);
    }

    [Fact]
    public void AcceptFriendshipRequest_PassingNonExistentFriendshipId_ShouldThrowNotFoundException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId nonExistentFriendshipId = Guid.NewGuid();

        // Act & Assert
        NotFoundException exception = Assert.Throws<NotFoundException>(() => sut.AcceptFriendshipRequest(nonExistentFriendshipId));
        Assert.Equal("Solicitação de amizade não encontrada.", exception.Message);
    }

    [Fact]
    public void AcceptFriendshipRequest_PassingFriendshipWhereUserIsRequester_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship friendshipRequest = new Friendship(sut.Id, invitedUserId);
        sut.SendFriendshipRequest(friendshipRequest);

        // Act & Assert
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.AcceptFriendshipRequest(friendshipRequest.Id));
        Assert.Equal("A solicitação de amizade não pertence a este usuário.", exception.Message);
    }

    [Fact]
    public void AcceptFriendshipRequest_PassingAlreadyAcceptedFriendship_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        User requester = new User("requester@example.com", "Requester");
        User invited = new User("invited@example.com", "Invited");

        Friendship friendshipRequest = new Friendship(requester.Id, invited.Id);
        requester.SendFriendshipRequest(friendshipRequest);
        AddFriendshipToAcceptedList(invited, friendshipRequest);
        invited.AcceptFriendshipRequest(friendshipRequest.Id);

        // Act & Assert
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => invited.AcceptFriendshipRequest(friendshipRequest.Id));
        Assert.Equal("A solicitação de amizade já foi respondida.", exception.Message);
    }

    [Fact]
    public void RejectFriendshipRequest_PassingValidPendingFriendship_ShouldRejectFriendship()
    {
        // Arrange
        User requester = new User("requester@example.com", "Requester");
        User invited = new User("invited@example.com", "Invited");

        Friendship friendshipRequest = new Friendship(requester.Id, invited.Id);
        requester.SendFriendshipRequest(friendshipRequest);
        AddFriendshipToAcceptedList(invited, friendshipRequest);

        // Act
        invited.RejectFriendshipRequest(friendshipRequest.Id);

        // Assert
        Assert.Equal(FriendshipStatus.Rejected, friendshipRequest.Status);
    }

    [Fact]
    public void RejectFriendshipRequest_PassingNonExistentFriendshipId_ShouldThrowNotFoundException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId nonExistentFriendshipId = Guid.NewGuid();

        // Act & Assert
        NotFoundException exception = Assert.Throws<NotFoundException>(() => sut.RejectFriendshipRequest(nonExistentFriendshipId));
        Assert.Equal("Solicitação de amizade não encontrada.", exception.Message);
    }

    [Fact]
    public void RejectFriendshipRequest_PassingFriendshipWhereUserIsRequester_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship friendshipRequest = new Friendship(sut.Id, invitedUserId);
        sut.SendFriendshipRequest(friendshipRequest);

        // Act & Assert
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.RejectFriendshipRequest(friendshipRequest.Id));
        Assert.Equal("A solicitação de amizade não pertence a este usuário.", exception.Message);
    }

    [Fact]
    public void RejectFriendshipRequest_PassingAlreadyRejectedFriendship_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        User requester = new User("requester@example.com", "Requester");
        User invited = new User("invited@example.com", "Invited");

        Friendship friendshipRequest = new Friendship(requester.Id, invited.Id);
        requester.SendFriendshipRequest(friendshipRequest);
        AddFriendshipToAcceptedList(invited, friendshipRequest);
        invited.RejectFriendshipRequest(friendshipRequest.Id);

        // Act & Assert
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => invited.RejectFriendshipRequest(friendshipRequest.Id));
        Assert.Equal("A solicitação de amizade já foi respondida.", exception.Message);
    }

    [Fact]
    public void GetFriendshipById_PassingExistingFriendshipId_ShouldReturnFriendship()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship friendshipRequest = new Friendship(sut.Id, invitedUserId);
        sut.SendFriendshipRequest(friendshipRequest);

        // Act
        Friendship result = sut.GetFriendshipById(friendshipRequest.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(friendshipRequest.Id, result.Id);
    }

    [Fact]
    public void GetFriendshipById_PassingNonExistentFriendshipId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId nonExistentFriendshipId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sut.GetFriendshipById(nonExistentFriendshipId));
    }

    [Fact]
    public void RemoveFriendship_PassingExistingRequestedFriendshipId_ShouldRemoveFriendship()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship friendshipRequest = new Friendship(sut.Id, invitedUserId);
        sut.SendFriendshipRequest(friendshipRequest);

        // Act
        sut.RemoveFriendship(friendshipRequest.Id);

        // Assert
        Assert.Empty(sut.Friendships);
    }

    [Fact]
    public void RemoveFriendship_PassingNonExistentFriendshipId_ShouldNotThrowException()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId nonExistentFriendshipId = Guid.NewGuid();

        // Act
        sut.RemoveFriendship(nonExistentFriendshipId);

        // Assert
        Assert.Empty(sut.Friendships);
    }

    [Fact]
    public void RemoveFriendship_PassingExistingAcceptedFriendshipId_ShouldRemoveFriendship()
    {
        // Arrange
        User requester = new User("requester@example.com", "Requester");
        User invited = new User("invited@example.com", "Invited");

        Friendship friendshipRequest = new Friendship(requester.Id, invited.Id);
        requester.SendFriendshipRequest(friendshipRequest);
        AddFriendshipToAcceptedList(invited, friendshipRequest);

        // Act
        invited.RemoveFriendship(friendshipRequest.Id);

        // Assert
        Assert.DoesNotContain(invited.Friendships, f => f.Id == friendshipRequest.Id);
    }

    [Fact]
    public void Friendships_AfterSendingMultipleRequests_ShouldContainAllFriendships()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");
        ReferenceId invitedUserId1 = Guid.NewGuid();
        ReferenceId invitedUserId2 = Guid.NewGuid();
        ReferenceId invitedUserId3 = Guid.NewGuid();

        Friendship request1 = new Friendship(sut.Id, invitedUserId1);
        Friendship request2 = new Friendship(sut.Id, invitedUserId2);
        Friendship request3 = new Friendship(sut.Id, invitedUserId3);

        // Act
        sut.SendFriendshipRequest(request1);
        sut.SendFriendshipRequest(request2);
        sut.SendFriendshipRequest(request3);

        // Assert
        Assert.Equal(3, sut.Friendships.Count);
        Assert.Contains(sut.Friendships, f => f.Id == request1.Id);
        Assert.Contains(sut.Friendships, f => f.Id == request2.Id);
        Assert.Contains(sut.Friendships, f => f.Id == request3.Id);
    }

    [Fact]
    public void Friendships_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        User sut = new User("user1@example.com", "User1");

        // Act
        IReadOnlyCollection<Friendship> friendships = sut.Friendships;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<Friendship>>(friendships);
    }

    public static IEnumerable<object[]> GetValidDisplayNames()
    {
        yield return new object[] { new string('a', UserConstants.MIN_DISPLAY_NAME_LENGTH) };
        yield return new object[] { new string('a', UserConstants.MIN_DISPLAY_NAME_LENGTH + 1) };
        yield return new object[] { new string('a', UserConstants.MAX_DISPLAY_NAME_LENGTH - 1) };
        yield return new object[] { new string('a', UserConstants.MAX_DISPLAY_NAME_LENGTH) };
        yield return new object[] { "ValidName" };
    }

    private static void AddFriendshipToAcceptedList(User user, Friendship friendship)
    {
        FieldInfo field = typeof(User).GetField("_friendshipAccepted", BindingFlags.NonPublic | BindingFlags.Instance);
        List<Friendship> acceptedList = (List<Friendship>)field.GetValue(user);
        acceptedList.Add(friendship);
    }
}