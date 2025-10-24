using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Users.RejectFriendshipRequest;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;

public sealed class RejectFriendshipRequestCommandHandler : CommandHandlerBase<RejectFriendshipRequestCommandHandler, RejectFriendshipRequestCommand, RejectFriendshipRequestCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserRepository _repository;

    public RejectFriendshipRequestCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RejectFriendshipRequestCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected async override Task<HandlerResult<RejectFriendshipRequestCommandResult>> InternalHandleAsync(RejectFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<RejectFriendshipRequestCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is null)
            return HandlerResult.CreateNotFound<RejectFriendshipRequestCommandResult>();

        user.RejectFriendshipRequest(request.FriendshipId);

        await _repository.SaveChangesAsync(cancellationToken);

        await _messageBus.SimplePublishAsync(new FriendshipRejectedEvent(request.FriendshipId), cancellationToken);

        return HandlerResult.Create(new RejectFriendshipRequestCommandResult());
    }
}