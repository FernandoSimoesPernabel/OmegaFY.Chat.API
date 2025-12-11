using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Domain.ValueObjects.Chat;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Tests.Unit.Domain.Entities.Chat;

public sealed class MessageFacts
{
    [Fact]
    public void Constructor_PassingValidParameters_ShouldCreateMessage()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType messageType = MessageType.Normal;
        MessageBody body = new MessageBody("Test message content");

        // Act
        Message sut = new Message(conversationId, senderMemberId, messageType, body);

        // Assert
        Assert.NotEqual(Guid.Empty, sut.Id.Value);
        Assert.Equal(conversationId, sut.ConversationId);
        Assert.Equal(senderMemberId, sut.SenderMemberId);
        Assert.Equal(messageType, sut.Type);
        Assert.Equal(body, sut.Body);
        Assert.True((DateTime.UtcNow - sut.SendDate).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_PassingValidParameters_ShouldSetSendDateToCurrentUtcTime()
    {
        // Arrange
        DateTime beforeCreation = DateTime.UtcNow;
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType messageType = MessageType.Normal;
        MessageBody body = new MessageBody("Test message content");

        // Act
        Message sut = new Message(conversationId, senderMemberId, messageType, body);
        DateTime afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(sut.SendDate >= beforeCreation);
        Assert.True(sut.SendDate <= afterCreation);
    }

    [Fact]
    public void Constructor_PassingInvalidMessageType_ShouldThrowDomainArgumentException()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType invalidMessageType = (MessageType)999;
        MessageBody body = new MessageBody("Test message content");

        // Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new Message(conversationId, senderMemberId, invalidMessageType, body));

        // Assert
        Assert.Equal("Tipo de mensagem inválido.", exception.Message);
    }

    [Theory]
    [InlineData("Simple message")]
    [InlineData("Message with special chars !@#$%^&*()")]
    [InlineData("A")]
    public void Constructor_PassingValidMessageBody_ShouldCreateMessage(string content)
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType messageType = MessageType.Normal;
        MessageBody body = new MessageBody(content);

        // Act
        Message sut = new Message(conversationId, senderMemberId, messageType, body);

        // Assert
        Assert.Equal(content.Trim(), sut.Body.Content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_PassingInvalidMessageBody_ShouldThrowDomainArgumentException(string invalidContent)
    {
        // Arrange & Act
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new MessageBody(invalidContent));

        // Assert
        Assert.Equal("Não foi informado nenhum conteudo para o corpo.", exception.Message);
    }

    [Fact]
    public void Constructor_PassingMessageBodyExceedingMaxLength_ShouldThrowDomainArgumentException()
    {
        // Arrange
        string longContent = new string('a', ChatConstants.MESSAGE_BODY_MAX_LENGTH + 1);

        // Act 
        DomainArgumentException exception = Assert.Throws<DomainArgumentException>(() => new MessageBody(longContent));

        // Assert
        Assert.Equal($"A mensagem não pode exceder o limite de {ChatConstants.MESSAGE_BODY_MAX_LENGTH} caracteres.", exception.Message);
    }

    [Fact]
    public void Constructor_PassingMessageBodyAtMaxLength_ShouldCreateMessage()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType messageType = MessageType.Normal;
        string maxLengthContent = new string('a', ChatConstants.MESSAGE_BODY_MAX_LENGTH);
        MessageBody body = new MessageBody(maxLengthContent);

        // Act
        Message sut = new Message(conversationId, senderMemberId, messageType, body);

        // Assert
        Assert.Equal(maxLengthContent, sut.Body.Content);
    }

    [Fact]
    public void Constructor_CreatingMultipleMessages_ShouldGenerateUniqueIds()
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType messageType = MessageType.Normal;
        MessageBody body = new MessageBody("Test message content");

        // Act
        Message message1 = new Message(conversationId, senderMemberId, messageType, body);
        Message message2 = new Message(conversationId, senderMemberId, messageType, body);

        // Assert
        Assert.NotEqual(message1.Id, message2.Id);
    }

    [Fact]
    public void MessageBody_ImplicitConversionToString_ShouldReturnContent()
    {
        // Arrange
        string content = "Test message content";
        MessageBody body = new MessageBody(content);

        // Act
        string result = body;

        // Assert
        Assert.Equal(content, result);
    }

    [Fact]
    public void MessageBody_ImplicitConversionFromString_ShouldCreateMessageBody()
    {
        // Arrange
        string content = "Test message content";

        // Act
        MessageBody body = content;

        // Assert
        Assert.Equal(content, body.Content);
    }

    [Fact]
    public void MessageBody_ToString_ShouldReturnContent()
    {
        // Arrange
        string content = "Test message content";
        MessageBody body = new MessageBody(content);

        // Act
        string result = body.ToString();

        // Assert
        Assert.Equal(content, result);
    }

    [Fact]
    public void MessageBody_WithLeadingAndTrailingSpaces_ShouldTrimContent()
    {
        // Arrange
        string content = "  Test message content  ";
        MessageBody body = new MessageBody(content);

        // Act
        string result = body.Content;

        // Assert
        Assert.Equal("Test message content", result);
    }

    [Theory]
    [MemberData(nameof(GetValidMessageContents))]
    public void Constructor_PassingVariousValidContents_ShouldCreateMessage(string content)
    {
        // Arrange
        ReferenceId conversationId = Guid.NewGuid();
        ReferenceId senderMemberId = Guid.NewGuid();
        MessageType messageType = MessageType.Normal;
        MessageBody body = new MessageBody(content);

        // Act
        Message sut = new Message(conversationId, senderMemberId, messageType, body);

        // Assert
        Assert.Equal(content.Trim(), sut.Body.Content);
    }

    public static IEnumerable<object[]> GetValidMessageContents()
    {
        yield return new object[] { "Short" };
        yield return new object[] { "Message with numbers 12345" };
        yield return new object[] { "Message with emoji 😀" };
        yield return new object[] { new string('a', 100) };
        yield return new object[] { new string('b', 500) };
        yield return new object[] { new string('c', ChatConstants.MESSAGE_BODY_MAX_LENGTH) };
    }
}