using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Users;

public sealed class FriendshipFacts
{
    [Fact]
    public void Constructor_PassingValidUserIds_ShouldCreateFriendshipWithPendingStatus()
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();

        // Act
        Friendship sut = new Friendship(requestingUserId, invitedUserId);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(requestingUserId, sut.RequestingUserId);
        Assert.Equal(invitedUserId, sut.InvitedUserId);
        Assert.Equal(FriendshipStatus.Pending, sut.Status);
        Assert.True((DateTime.UtcNow - sut.StartedDate).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_CreatingMultipleFriendships_ShouldGenerateUniqueIds()
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();

        // Act
        Friendship friendship1 = new Friendship(requestingUserId, invitedUserId);
        Friendship friendship2 = new Friendship(requestingUserId, invitedUserId);

        // Assert
        Assert.NotEqual(friendship1.Id, friendship2.Id);
    }

    [Fact]
    public void Accept_PendingFriendship_ShouldChangeStatusToAccepted()
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship sut = new Friendship(requestingUserId, invitedUserId);

        // Act
        sut.Accept();

        // Assert
        Assert.Equal(FriendshipStatus.Accepted, sut.Status);
    }

    [Fact]
    public void Accept_AcceptedFriendship_ShouldRemainAccepted()
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship sut = new Friendship(requestingUserId, invitedUserId);
        sut.Accept();

        // Act
        sut.Accept();

        // Assert
        Assert.Equal(FriendshipStatus.Accepted, sut.Status);
    }

    [Fact]
    public void Reject_PendingFriendship_ShouldChangeStatusToRejected()
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship sut = new Friendship(requestingUserId, invitedUserId);

        // Act
        sut.Reject();

        // Assert
        Assert.Equal(FriendshipStatus.Rejected, sut.Status);
    }

    [Fact]
    public void Reject_RejectedFriendship_ShouldRemainRejected()
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship sut = new Friendship(requestingUserId, invitedUserId);
        sut.Reject();

        // Act
        sut.Reject();

        // Assert
        Assert.Equal(FriendshipStatus.Rejected, sut.Status);
    }

    [Theory]
    [InlineData(FriendshipStatus.Pending)]
    [InlineData(FriendshipStatus.Accepted)]
    [InlineData(FriendshipStatus.Rejected)]
    public void Accept_FromAnyStatus_ShouldChangeToAccepted(FriendshipStatus initialStatus)
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship sut = new Friendship(requestingUserId, invitedUserId);

        if (initialStatus == FriendshipStatus.Accepted)
            sut.Accept();
        else if (initialStatus == FriendshipStatus.Rejected)
            sut.Reject();

        // Act
        sut.Accept();

        // Assert
        Assert.Equal(FriendshipStatus.Accepted, sut.Status);
    }

    [Theory]
    [InlineData(FriendshipStatus.Pending)]
    [InlineData(FriendshipStatus.Accepted)]
    [InlineData(FriendshipStatus.Rejected)]
    public void Reject_FromAnyStatus_ShouldChangeToRejected(FriendshipStatus initialStatus)
    {
        // Arrange
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();
        Friendship sut = new Friendship(requestingUserId, invitedUserId);

        if (initialStatus == FriendshipStatus.Accepted)
            sut.Accept();
        else if (initialStatus == FriendshipStatus.Rejected)
            sut.Reject();

        // Act
        sut.Reject();

        // Assert
        Assert.Equal(FriendshipStatus.Rejected, sut.Status);
    }

    [Fact]
    public void Constructor_ShouldSetStartedDateToCurrentUtcTime()
    {
        // Arrange
        DateTime beforeCreation = DateTime.UtcNow;
        ReferenceId requestingUserId = Guid.NewGuid();
        ReferenceId invitedUserId = Guid.NewGuid();

        // Act
        Friendship sut = new Friendship(requestingUserId, invitedUserId);
        DateTime afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(sut.StartedDate >= beforeCreation);
        Assert.True(sut.StartedDate <= afterCreation);
    }
}