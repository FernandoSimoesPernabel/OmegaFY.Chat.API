using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Users.AcceptFriendshipRequest;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;

public sealed class AcceptFriendshipRequestCommandHandler : CommandHandlerBase<AcceptFriendshipRequestCommandHandler, AcceptFriendshipRequestCommand, AcceptFriendshipRequestCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserRepository _repository;

    public AcceptFriendshipRequestCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<AcceptFriendshipRequestCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected async override Task<HandlerResult<AcceptFriendshipRequestCommandResult>> InternalHandleAsync(AcceptFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<AcceptFriendshipRequestCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is null)
            return HandlerResult.CreateNotFound<AcceptFriendshipRequestCommandResult>();

        user.AcceptFriendshipRequest(request.FriendshipId);

        await _repository.SaveChangesAsync(cancellationToken);

        Friendship friendship = user.GetFriendshipById(request.FriendshipId);

        await _messageBus.SimplePublishAsync(new FriendshipAcceptedEvent(friendship.Id, friendship.RequestingUserId, friendship.InvitedUserId), cancellationToken);

        return HandlerResult.Create(new AcceptFriendshipRequestCommandResult());
    }
}