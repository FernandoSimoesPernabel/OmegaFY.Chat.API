using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Chat;

public sealed class MemberMessageFacts
{
    [Fact]
    public void Constructor_PassingValidParameters_ShouldCreateMemberMessageWithUnreadStatus()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();

        // Act
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(messageId, sut.MessageId);
        Assert.Equal(senderMemberId, sut.SenderMemberId);
        Assert.Equal(destinationMemberId, sut.DestinationMemberId);
        Assert.Equal(MemberMessageStatus.Unread, sut.Status);
        Assert.True((DateTime.UtcNow - sut.DeliveryDate).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_ShouldSetDeliveryDateToCurrentUtcTime()
    {
        // Arrange
        DateTime beforeCreation = DateTime.UtcNow;
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();

        // Act
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        DateTime afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(sut.DeliveryDate >= beforeCreation);
        Assert.True(sut.DeliveryDate <= afterCreation);
    }

    [Fact]
    public void Read_UnreadMessage_ShouldChangeStatusToRead()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Act
        sut.Read();

        // Assert
        Assert.Equal(MemberMessageStatus.Read, sut.Status);
    }

    [Fact]
    public void Read_AlreadyReadMessage_ShouldRemainRead()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Read();

        // Act
        sut.Read();

        // Assert
        Assert.Equal(MemberMessageStatus.Read, sut.Status);
    }

    [Fact]
    public void Read_DeletedMessage_ShouldNotChangeStatus()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Delete();

        // Act
        sut.Read();

        // Assert
        Assert.Equal(MemberMessageStatus.Deleted, sut.Status);
    }

    [Fact]
    public void Delete_UnreadMessage_ShouldChangeStatusToDeleted()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Act
        sut.Delete();

        // Assert
        Assert.Equal(MemberMessageStatus.Deleted, sut.Status);
    }

    [Fact]
    public void Delete_ReadMessage_ShouldChangeStatusToDeleted()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Read();

        // Act
        sut.Delete();

        // Assert
        Assert.Equal(MemberMessageStatus.Deleted, sut.Status);
    }

    [Fact]
    public void Delete_AlreadyDeletedMessage_ShouldRemainDeleted()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Delete();

        // Act
        sut.Delete();

        // Assert
        Assert.Equal(MemberMessageStatus.Deleted, sut.Status);
    }

    [Fact]
    public void IsUnread_UnreadMessage_ShouldReturnTrue()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Act
        bool result = sut.IsUnread();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUnread_ReadMessage_ShouldReturnFalse()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Read();

        // Act
        bool result = sut.IsUnread();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRead_ReadMessage_ShouldReturnTrue()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Read();

        // Act
        bool result = sut.IsRead();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRead_UnreadMessage_ShouldReturnFalse()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Act
        bool result = sut.IsRead();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsDeleted_DeletedMessage_ShouldReturnTrue()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        sut.Delete();

        // Act
        bool result = sut.IsDeleted();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDeleted_UnreadMessage_ShouldReturnFalse()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Act
        bool result = sut.IsDeleted();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(MemberMessageStatus.Unread)]
    [InlineData(MemberMessageStatus.Read)]
    [InlineData(MemberMessageStatus.Deleted)]
    public void Delete_FromAnyStatus_ShouldChangeToDeleted(MemberMessageStatus initialStatus)
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();
        MemberMessage sut = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        if (initialStatus == MemberMessageStatus.Read)
            sut.Read();
        else if (initialStatus == MemberMessageStatus.Deleted)
            sut.Delete();

        // Act
        sut.Delete();

        // Assert
        Assert.Equal(MemberMessageStatus.Deleted, sut.Status);
    }

    [Fact]
    public void Constructor_CreatingMultipleMemberMessages_ShouldGenerateUniqueIds()
    {
        // Arrange
        ReferenceId messageId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        ReferenceId destinationMemberId = Guid.NewGuid();

        // Act
        MemberMessage memberMessage1 = new MemberMessage(messageId, senderMemberId, destinationMemberId);
        MemberMessage memberMessage2 = new MemberMessage(messageId, senderMemberId, destinationMemberId);

        // Assert
        Assert.NotEqual(memberMessage1.Id, memberMessage2.Id);
    }
}