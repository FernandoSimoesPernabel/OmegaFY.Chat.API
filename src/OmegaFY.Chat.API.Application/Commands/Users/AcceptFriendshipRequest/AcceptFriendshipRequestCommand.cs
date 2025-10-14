using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;

public sealed record class AcceptFriendshipRequestCommand : ICommand
{
    public Guid FriendshipId { get; init; }

    public AcceptFriendshipRequestCommand() { }

    public AcceptFriendshipRequestCommand(Guid friendshipId) => FriendshipId = friendshipId;
}

public sealed record class AcceptFriendshipRequestCommandResult : ICommandResult
{
}

public sealed class AcceptFriendshipRequestCommandHandler : CommandHandlerBase<AcceptFriendshipRequestCommandHandler, AcceptFriendshipRequestCommand, AcceptFriendshipRequestCommandResult>
{
    public AcceptFriendshipRequestCommandHandler(
        IHostEnvironment hostEnvironment, 
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<AcceptFriendshipRequestCommand> validator, 
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<AcceptFriendshipRequestCommandResult>> InternalHandleAsync(AcceptFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}