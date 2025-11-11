using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;

namespace OmegaFY.Chat.API.Application.Events.Chat.SendMessage;

internal class ReplicateMessageToMembersEventHandler : EventHandlerHandlerBase<MessageSentEvent>
{
    private readonly IConversationRepository _conversationRepository;

    private readonly IMessageRepository _messageRepository;

    private readonly IMemberMessageRepository _memberMessageRepository;

    public ReplicateMessageToMembersEventHandler(IConversationRepository conversationRepository, IMessageRepository messageRepository)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
    }

    protected override async Task HandleAsync(MessageSentEvent @event, CancellationToken cancellationToken)
    {
        Message message = await _messageRepository.GetMessageByIdAsync(@event.MessageId, cancellationToken);

        Conversation conversation = await _conversationRepository.GetConversationByIdAsync(message.ConversationId, cancellationToken);

        foreach (Member conversationMember in conversation.Members)
        {
            MemberMessage memberMessage = new MemberMessage(message.Id, message.SenderMemberId, conversationMember.Id);
            await _memberMessageRepository.CreateMemberMessageAsync(memberMessage, cancellationToken);
        }

        await _memberMessageRepository.SaveChangesAsync(cancellationToken);

        //TODO SignalR de nova mensagem para os membros da conversa
    }
}