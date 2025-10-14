using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;

public sealed record class RemoveFriendshipCommand : ICommand
{
    public Guid FriendshipId { get; init; }

    public RemoveFriendshipCommand() { }

    public RemoveFriendshipCommand(Guid friendshipId) => FriendshipId = friendshipId;
}

public sealed record class RemoveFriendshipCommandResult : ICommandResult
{
}

public sealed class RemoveFriendshipCommandHandler : CommandHandlerBase<RemoveFriendshipCommandHandler, RemoveFriendshipCommand, RemoveFriendshipCommandResult>
{
    public RemoveFriendshipCommandHandler(
        IHostEnvironment hostEnvironment, 
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider, 
        IValidator<RemoveFriendshipCommand> validator, 
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<RemoveFriendshipCommandResult>> InternalHandleAsync(RemoveFriendshipCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}