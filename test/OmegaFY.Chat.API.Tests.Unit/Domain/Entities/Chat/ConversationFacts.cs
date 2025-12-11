using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Chat;

public sealed class ConversationFacts
{
    [Fact]
    public void StartMemberToMemberConversation_PassingValidUserIds_ShouldCreateConversation()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();

        // Act
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(ConversationType.MemberToMember, sut.Type);
        Assert.Equal(ConversationStatus.Open, sut.Status);
        Assert.Null(sut.GroupConfig);
        Assert.Equal(2, sut.Members.Count);
        Assert.True((DateTime.UtcNow - sut.CreatedDate).TotalSeconds < 1);
    }

    [Fact]
    public void StartMemberToMemberConversation_PassingSameUserId_ShouldThrowDomainArgumentException()
    {
        // Arrange
        ReferenceId userId = Guid.NewGuid();

        // Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => Conversation.StartMemberToMemberConversation(userId, userId));

        // Assert
        Assert.Equal("Não é possível criar uma conversa entre o mesmo usuário.", exception.Message);
    }

    [Fact]
    public void CreateGroupChat_PassingValidParameters_ShouldCreateGroupConversation()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = "Test Group";
        byte maxNumberOfMembers = 10;

        // Act
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, groupName, maxNumberOfMembers);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(ConversationType.GroupChat, sut.Type);
        Assert.Equal(ConversationStatus.Open, sut.Status);
        Assert.NotNull(sut.GroupConfig);
        Assert.Equal(groupName, sut.GroupConfig.GroupName);
        Assert.Equal(maxNumberOfMembers, sut.GroupConfig.MaxNumberOfMembers);
        Assert.Single(sut.Members);
        Assert.True((DateTime.UtcNow - sut.CreatedDate).TotalSeconds < 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateGroupChat_PassingInvalidGroupName_ShouldThrowDomainArgumentException(string invalidGroupName)
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        byte maxNumberOfMembers = 10;

        // Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => Conversation.CreateGroupChat(createdByUserId, invalidGroupName, maxNumberOfMembers));

        // Assert
        Assert.Equal("O nome do grupo não foi informado.", exception.Message);
    }

    [Fact]
    public void CreateGroupChat_PassingGroupNameExceedingMaxLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = new string('a', ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH + 1);
        byte maxNumberOfMembers = 10;

        // Act & Assert
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() =>
            Conversation.CreateGroupChat(createdByUserId, groupName, maxNumberOfMembers));
        Assert.Equal($"O nome do grupo não pode exceder {ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH} caracteres.", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(200)]
    public void CreateGroupChat_PassingMaxNumberOfMembersOutOfRange_ShouldAdjustToValidRange(byte invalidMax)
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        string groupName = "Test Group";

        // Act
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, groupName, invalidMax);

        // Assert
        Assert.True(sut.GroupConfig.MaxNumberOfMembers >= ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS);
        Assert.True(sut.GroupConfig.MaxNumberOfMembers <= ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS);
    }

    [Fact]
    public void AddMemberToGroup_InGroupChat_ShouldAddMember()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, "Test Group", 10);
        ReferenceId newMemberUserId = Guid.NewGuid();

        // Act
        sut.AddMemberToGroup(newMemberUserId);

        // Assert
        Assert.Equal(2, sut.Members.Count);
        Assert.True(sut.IsUserInConversation(newMemberUserId));
    }

    [Fact]
    public void AddMemberToGroup_InMemberToMemberConversation_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        ReferenceId newMemberUserId = Guid.NewGuid();

        // Act & Assert
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.AddMemberToGroup(newMemberUserId));

        // Assert
        Assert.Equal("Não é possível adicionar membros em uma conversa que não é em grupo.", exception.Message);
    }

    [Fact]
    public void AddMemberToGroup_AddingExistingMember_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, "Test Group", 10);

        // Act
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.AddMemberToGroup(createdByUserId));

        // Assert
        Assert.Equal("Usuário já é membro da conversa.", exception.Message);
    }

    [Fact]
    public void RemoveMemberFromGroup_InGroupChat_ShouldRemoveMember()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, "Test Group", 10);
        ReferenceId newMemberUserId = Guid.NewGuid();
        sut.AddMemberToGroup(newMemberUserId);
        Member memberToRemove = sut.GetMemberByUserId(newMemberUserId);

        // Act
        sut.RemoveMemberFromGroup(memberToRemove.Id);

        // Assert
        Assert.Single(sut.Members);
        Assert.False(sut.IsUserInConversation(newMemberUserId));
    }

    [Fact]
    public void RemoveMemberFromGroup_InMemberToMemberConversation_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        Member memberToRemove = sut.GetMemberByUserId(memberOneUserId);

        // Act
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.RemoveMemberFromGroup(memberToRemove.Id));

        // Assert
        Assert.Equal("Não é possível remover membros em uma conversa que não é em grupo.", exception.Message);
    }

    [Fact]
    public void RemoveMemberFromGroup_RemovingNonExistentMember_ShouldNotThrowException()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, "Test Group", 10);
        ReferenceId nonExistentMemberId = Guid.NewGuid();

        // Act
        sut.RemoveMemberFromGroup(nonExistentMemberId);

        // Assert
        Assert.Single(sut.Members);
    }

    [Fact]
    public void ChangeGroupConfig_InGroupChat_ShouldUpdateConfiguration()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, "Old Group Name", 10);
        string newGroupName = "New Group Name";
        byte newMaxNumberOfMembers = 20;

        // Act
        sut.ChangeGroupConfig(newGroupName, newMaxNumberOfMembers);

        // Assert
        Assert.Equal(newGroupName, sut.GroupConfig.GroupName);
        Assert.Equal(newMaxNumberOfMembers, sut.GroupConfig.MaxNumberOfMembers);
    }

    [Fact]
    public void ChangeGroupConfig_InMemberToMemberConversation_ShouldThrowDomainInvalidOperationException()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);

        // Act
        DomainInvalidOperationException exception = Assert.Throws<DomainInvalidOperationException>(() => sut.ChangeGroupConfig("New Name", 10));

        // Assert
        Assert.Equal("Não é possível alterar a configuração de uma conversa que não é em grupo.", exception.Message);
    }

    [Fact]
    public void ChangeGroupConfig_SettingMaxMembersBelowCurrentCount_ShouldThrowDomainArgumentException()
    {
        // Arrange
        ReferenceId createdByUserId = Guid.NewGuid();
        Conversation sut = Conversation.CreateGroupChat(createdByUserId, "Test Group", 10);
        sut.AddMemberToGroup(Guid.NewGuid());
        sut.AddMemberToGroup(Guid.NewGuid());

        // Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => sut.ChangeGroupConfig("Test Group", 2));

        // Assert
        Assert.Equal("O número máximo de membros não pode ser menor que o número atual de membros.", exception.Message);
    }

    [Fact]
    public void IsUserInConversation_WithExistingUser_ShouldReturnTrue()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);

        // Act
        bool result = sut.IsUserInConversation(memberOneUserId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUserInConversation_WithNonExistingUser_ShouldReturnFalse()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        ReferenceId nonExistentUserId = Guid.NewGuid();

        // Act
        bool result = sut.IsUserInConversation(nonExistentUserId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetMemberByUserId_WithExistingUser_ShouldReturnMember()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);

        // Act
        Member result = sut.GetMemberByUserId(memberOneUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(memberOneUserId, result.UserId);
    }

    [Fact]
    public void GetMemberByUserId_WithNonExistingUser_ShouldReturnNull()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        ReferenceId nonExistentUserId = Guid.NewGuid();

        // Act
        Member result = sut.GetMemberByUserId(nonExistentUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetMemberByMemberId_WithExistingMember_ShouldReturnMember()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        Member existingMember = sut.GetMemberByUserId(memberOneUserId);

        // Act
        Member result = sut.GetMemberByMemberId(existingMember.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingMember.Id, result.Id);
    }

    [Fact]
    public void GetMemberByMemberId_WithNonExistingMember_ShouldReturnNull()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        ReferenceId nonExistentMemberId = Guid.NewGuid();

        // Act
        Member result = sut.GetMemberByMemberId(nonExistentMemberId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Members_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();
        Conversation sut = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);

        // Act
        IReadOnlyCollection<Member> members = sut.Members;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<Member>>(members);
    }

    [Fact]
    public void Constructor_CreatingMultipleConversations_ShouldGenerateUniqueIds()
    {
        // Arrange
        ReferenceId memberOneUserId = Guid.NewGuid();
        ReferenceId memberTwoUserId = Guid.NewGuid();

        // Act
        Conversation conversation1 = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);
        Conversation conversation2 = Conversation.StartMemberToMemberConversation(memberOneUserId, memberTwoUserId);

        // Assert
        Assert.NotEqual(conversation1.Id, conversation2.Id);
    }
}