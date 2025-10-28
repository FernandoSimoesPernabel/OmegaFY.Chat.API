using OmegaFY.Chat.API.Application.Events.Base;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;

namespace OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;

internal sealed class InitiateConversationEventHandler : EventHandlerHandlerBase<FriendshipAcceptedEvent>
{
    private readonly IUserQueryProvider _userQueryProvider;

    private readonly IConversationRepository _conversationRepository;

    public InitiateConversationEventHandler(IUserQueryProvider userQueryProvider, IConversationRepository conversationRepository)
    {
        _userQueryProvider = userQueryProvider;
        _conversationRepository = conversationRepository;
    }

    protected override async Task HandleAsync(FriendshipAcceptedEvent @event, CancellationToken cancellationToken)
    {
        FriendshipModel friendship = await _userQueryProvider.GetFriendshipByIdAsync(@event.FriendshipId, cancellationToken);

        Conversation memberToMemberConversation = Conversation.StartMemberToMemberConversation(friendship.RequestingUserId, friendship.InvitedUserId);

        await _conversationRepository.CreateConversationAsync(memberToMemberConversation, cancellationToken);

        await _conversationRepository.SaveChangesAsync(cancellationToken);

        //TODO SignalR de nova conversa
    }
}