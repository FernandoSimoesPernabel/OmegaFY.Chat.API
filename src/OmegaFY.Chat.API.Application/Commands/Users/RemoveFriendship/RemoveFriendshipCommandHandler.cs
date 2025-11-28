using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Users.RemoveFriendship;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;

public sealed class RemoveFriendshipCommandHandler : CommandHandlerBase<RemoveFriendshipCommandHandler, RemoveFriendshipCommand, RemoveFriendshipCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserRepository _repository;

    public RemoveFriendshipCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RemoveFriendshipCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected async override Task<HandlerResult<RemoveFriendshipCommandResult>> InternalHandleAsync(RemoveFriendshipCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<RemoveFriendshipCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is not null)
        {
            user.RemoveFriendship(request.FriendshipId);

            await _repository.SaveChangesAsync(cancellationToken);

            await _messageBus.SimplePublishAsync(new FriendshipRemovedEvent(request.FriendshipId), cancellationToken);
        }

        return HandlerResult.Create(new RemoveFriendshipCommandResult());
    }
}