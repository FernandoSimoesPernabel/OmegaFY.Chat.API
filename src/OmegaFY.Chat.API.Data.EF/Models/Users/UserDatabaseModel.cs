namespace OmegaFY.Chat.API.Data.EF.Models.Users;

internal sealed class UserDatabaseModel
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string DisplayName { get; set; }

    public ICollection<ChatDatabaseModel> Chats { get; set; } = [];
}

internal sealed class ChatDatabaseModel
{
    public Guid Id { get; set; }

    public object Type { get; set; } //ChatType

    public DateTime CreatedDate { get; set; }

    public GroupChatConfigDatabaseModel GroupChatConfig { get; set; }

    public ICollection<MemberDatabaseModel> Members { get; set; } = [];

    public ICollection<MessageDatabaseModel> Messages { get; set; } = [];
}

internal sealed class GroupChatConfigDatabaseModel
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public string Name { get; set; }

    public ChatDatabaseModel Chat { get; set; }
}

internal sealed class MemberDatabaseModel
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public ChatDatabaseModel Chat { get; set; }

    public UserDatabaseModel User { get; set; }
}

internal sealed class MessageDatabaseModel
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid SenderUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public object Type { get; set; } //MessageType

    public string Content { get; set; }

    public ChatDatabaseModel Chat { get; set; }

    public UserDatabaseModel SenderUser { get; set; }

    public ICollection<UserChatMessageDatabaseModel> UserChatMessages { get; set; } = [];
}

internal sealed class UserChatMessageDatabaseModel
{
    public Guid Id { get; set; }

    public Guid MessageId { get; set; }

    public Guid SenderUserId { get; set; }

    public Guid DestinationUserId { get; set; }

    public object Type { get; set; } //Algum type para se eu deletei a minha so

    public MessageDatabaseModel Message { get; set; }

    public UserDatabaseModel SenderUser { get; set; }

    public UserDatabaseModel DestinationUser { get; set; }
}