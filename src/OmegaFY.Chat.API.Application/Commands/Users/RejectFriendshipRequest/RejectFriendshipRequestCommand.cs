using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;

public sealed record class RejectFriendshipRequestCommand : ICommand
{
    public Guid FriendshipId { get; init; }

    public RejectFriendshipRequestCommand() { }

    public RejectFriendshipRequestCommand(Guid friendshipId) => FriendshipId = friendshipId;
}

public sealed record class RejectFriendshipRequestCommandResult : ICommandResult
{
}

public sealed class RejectFriendshipRequestCommandHandler : CommandHandlerBase<RejectFriendshipRequestCommandHandler, RejectFriendshipRequestCommand, RejectFriendshipRequestCommandResult>
{
    public RejectFriendshipRequestCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RejectFriendshipRequestCommand> validator, 
        IMessageBus messageBus) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
    }

    protected override Task<HandlerResult<RejectFriendshipRequestCommandResult>> InternalHandleAsync(RejectFriendshipRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}