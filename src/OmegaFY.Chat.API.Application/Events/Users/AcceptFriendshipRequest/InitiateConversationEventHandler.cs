using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;

namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

internal sealed class InitiateConversationEventHandler : EventHandlerHandlerBase<FriendshipAcceptedEvent>
{

    private readonly IConversationRepository _repository;

    public InitiateConversationEventHandler(IConversationRepository repository) => _repository = repository;

    protected override async Task HandleAsync(FriendshipAcceptedEvent @event, CancellationToken cancellationToken)
    {
        Conversation memberToMemberConversation = Conversation.StartMemberToMemberConversation(@event.RequestingUserId, @event.InvitedUserId);

        await _repository.CreateConversationAsync(memberToMemberConversation, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        //TODO SignalR de nova conversa
    }
}