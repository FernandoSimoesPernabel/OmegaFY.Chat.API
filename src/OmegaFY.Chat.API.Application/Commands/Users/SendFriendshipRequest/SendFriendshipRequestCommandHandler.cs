using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Users.SendFriendshipRequest;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;

public sealed class SendFriendshipRequestCommandHandler : CommandHandlerBase<SendFriendshipRequestCommandHandler, SendFriendshipRequestCommand, SendFriendshipRequestCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserRepository _repository;

    public SendFriendshipRequestCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<SendFriendshipRequestCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected async override Task<HandlerResult<SendFriendshipRequestCommandResult>> InternalHandleAsync(SendFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<SendFriendshipRequestCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is null)
            return HandlerResult.CreateNotFound<SendFriendshipRequestCommandResult>();

        Friendship friendshipRequest = new Friendship(_userInformation.CurrentRequestUserId.Value, request.InvitedUserId);

        user.SendFriendshipRequest(friendshipRequest);

        await _repository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(
            new FriendshipRequestedEvent(friendshipRequest.Id, friendshipRequest.RequestingUserId, friendshipRequest.InvitedUserId, friendshipRequest.StartedDate), 
            cancellationToken);

        return HandlerResult.Create(new SendFriendshipRequestCommandResult(friendshipRequest.Id));
    }
}