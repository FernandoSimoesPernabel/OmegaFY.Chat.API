using FluentValidation;
using Microsoft.Extensions.Hosting;
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
            HandlerResult.CreateUnauthorized<SendFriendshipRequestCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is null)
            return HandlerResult.CreateNotFound<SendFriendshipRequestCommandResult>();

        Friendship friendship = user.SendFriendshipRequest(request.InvitedUserId);

        await _repository.SaveChangesAsync(cancellationToken);

        return HandlerResult.Create(new SendFriendshipRequestCommandResult(friendship.Id));
    }
}