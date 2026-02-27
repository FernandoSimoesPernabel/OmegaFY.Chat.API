using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.Hubs;

namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

internal class ReplicateMessageToMembersEventHandler : EventHandlerHandlerBase<MessageSentEvent>
{
    private readonly IConversationRepository _conversationRepository;

    private readonly IMessageRepository _messageRepository;

    private readonly IMemberMessageRepository _memberMessageRepository;

    private readonly IChatNotificationProvider _chatNotificationProvider;

    public ReplicateMessageToMembersEventHandler(
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository,
        IMemberMessageRepository memberMessageRepository,
        IChatNotificationProvider chatNotificationProvider)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _memberMessageRepository = memberMessageRepository;
        _chatNotificationProvider = chatNotificationProvider;
    }

    protected async override Task HandleAsync(MessageSentEvent @event, CancellationToken cancellationToken)
    {
        Message message = await _messageRepository.GetMessageByIdAsync(@event.MessageId, cancellationToken);

        Conversation conversation = await _conversationRepository.GetConversationByIdAsync(@event.ConversationId, cancellationToken);

        foreach (Member conversationMember in conversation.Members)
        {
            MemberMessage memberMessage = new MemberMessage(message.Id, message.SenderMemberId, conversationMember.Id);
            await _memberMessageRepository.CreateMemberMessageAsync(memberMessage, cancellationToken);
        }

        await _memberMessageRepository.SaveChangesAsync(cancellationToken);

        await Task.WhenAll(conversation.Members.Where(member => !member.IsUser(@event.SenderUserId)).Select(member => _chatNotificationProvider.MessageReceivedAsync(member.UserId, @event.ConversationId, @event.MessageId)));
    }
}