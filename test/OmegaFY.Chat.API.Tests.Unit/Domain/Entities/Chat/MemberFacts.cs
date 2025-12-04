using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Chat;

public sealed class MemberFacts
{
    [Fact]
    public void Constructor_PassingValidParameters_ShouldCreateMember()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId userId = Guid.NewGuid();

        // Act
        Member sut = new Member(conversationId, userId);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(conversationId, sut.ConversationId);
        Assert.Equal(userId, sut.UserId);
        Assert.True((DateTime.UtcNow - sut.JoinedDate).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_ShouldSetJoinedDateToCurrentUtcTime()
    {
        // Arrange
        DateTime beforeCreation = DateTime.UtcNow;
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId userId = Guid.NewGuid();

        // Act
        Member sut = new Member(conversationId, userId);
        DateTime afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(sut.JoinedDate >= beforeCreation);
        Assert.True(sut.JoinedDate <= afterCreation);
    }

    [Fact]
    public void Constructor_CreatingMultipleMembers_ShouldGenerateUniqueIds()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId userId = Guid.NewGuid();

        // Act
        Member member1 = new Member(conversationId, userId);
        Member member2 = new Member(conversationId, userId);

        // Assert
        Assert.NotEqual(member1.Id, member2.Id);
    }

    [Fact]
    public void Constructor_CreatingMembersForDifferentConversations_ShouldMaintainSeparateIds()
    {
        // Arrange
        ReferenceId conversationId1 = Guid.NewGuid();
        ReferenceId conversationId2 = Guid.NewGuid();
        ReferenceId userId = Guid.NewGuid();

        // Act
        Member member1 = new Member(conversationId1, userId);
        Member member2 = new Member(conversationId2, userId);

        // Assert
        Assert.NotEqual(member1.Id, member2.Id);
        Assert.Equal(conversationId1, member1.ConversationId);
        Assert.Equal(conversationId2, member2.ConversationId);
        Assert.Equal(userId, member1.UserId);
        Assert.Equal(userId, member2.UserId);
    }
}