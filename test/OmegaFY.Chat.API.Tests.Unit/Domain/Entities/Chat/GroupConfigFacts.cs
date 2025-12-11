using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Chat;

public sealed class GroupConfigFacts
{
    [Fact]
    public void Constructor_PassingValidParameters_ShouldCreateGroupConfig()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = "Test Group";
        byte maxNumberOfMembers = 10;

        // Act
        GroupConfig sut = new GroupConfig(conversationId, createdByUserId, groupName, maxNumberOfMembers);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(conversationId, sut.ConversationId);
        Assert.Equal(createdByUserId, sut.CreatedByUserId);
        Assert.Equal(groupName, sut.GroupName);
        Assert.Equal(maxNumberOfMembers, sut.MaxNumberOfMembers);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_PassingInvalidGroupName_ShouldThrowDomainArgumentException(string invalidGroupName)
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        byte maxNumberOfMembers = 10;

        // Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new GroupConfig(conversationId, createdByUserId, invalidGroupName, maxNumberOfMembers));

        // Assert
        Assert.Equal("O nome do grupo não foi informado.", exception.Message);
    }

    [Fact]
    public void Constructor_PassingGroupNameExceedingMaxLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = new string('a', ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH + 1);
        byte maxNumberOfMembers = 10;

        // Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new GroupConfig(conversationId, createdByUserId, groupName, maxNumberOfMembers));

        // Assert
        Assert.Equal($"O nome do grupo não pode exceder {ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH} caracteres.", exception.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void Constructor_PassingValidMaxNumberOfMembers_ShouldAcceptValue(byte maxMembers)
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = "Test Group";

        // Act
        GroupConfig sut = new GroupConfig(conversationId, createdByUserId, groupName, maxMembers);

        // Assert
        Assert.Equal(maxMembers, sut.MaxNumberOfMembers);
    }

    [Theory]
    [InlineData(0, ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS)]
    [InlineData(101, ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS)]
    [InlineData(200, ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS)]
    public void Constructor_PassingMaxNumberOfMembersOutOfRange_ShouldClampToValidRange(byte invalidMax, byte expectedMax)
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = "Test Group";

        // Act
        GroupConfig sut = new GroupConfig(conversationId, createdByUserId, groupName, invalidMax);

        // Assert
        Assert.Equal(expectedMax, sut.MaxNumberOfMembers);
    }

    [Theory]
    [MemberData(nameof(GetValidGroupNames))]
    public void Constructor_PassingValidGroupNameLengths_ShouldCreateGroupConfig(string groupName)
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        byte maxNumberOfMembers = 10;

        // Act
        GroupConfig sut = new GroupConfig(conversationId, createdByUserId, groupName, maxNumberOfMembers);

        // Assert
        Assert.Equal(groupName, sut.GroupName);
    }

    [Fact]
    public void Constructor_CreatingMultipleGroupConfigs_ShouldGenerateUniqueIds()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = "Test Group";
        byte maxNumberOfMembers = 10;

        // Act
        GroupConfig config1 = new GroupConfig(conversationId, createdByUserId, groupName, maxNumberOfMembers);
        GroupConfig config2 = new GroupConfig(conversationId, createdByUserId, groupName, maxNumberOfMembers);

        // Assert
        Assert.NotEqual(config1.Id, config2.Id);
    }

    public static IEnumerable<object[]> GetValidGroupNames()
    {
        yield return new object[] { "A" };
        yield return new object[] { "Test Group" };
        yield return new object[] { new string('a', ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH) };
        yield return new object[] { "Group Name With Spaces" };
        yield return new object[] { "GroupNameWithoutSpaces" };
    }
}