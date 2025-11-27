using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Chat.AddMemberToGroup;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Domain.Entities.Chat;
using OmegaFY.Chat.API.Domain.Repositories.Chat;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed class AddMemberToGroupCommandHandler : CommandHandlerBase<AddMemberToGroupCommandHandler, AddMemberToGroupCommand, AddMemberToGroupCommandResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IConversationRepository _repository;

    public AddMemberToGroupCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<AddMemberToGroupCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation,
        IConversationRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
        _repository = repository;
    }

    protected override async Task<HandlerResult<AddMemberToGroupCommandResult>> InternalHandleAsync(AddMemberToGroupCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<AddMemberToGroupCommandResult>();

        Conversation conversation = await _repository.GetConversationByIdAsync(request.ConversationId, cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<AddMemberToGroupCommandResult>();

        if (!conversation.IsUserInConversation(_userInformation.CurrentRequestUserId.Value))
            return HandlerResult.CreateUnauthorized<AddMemberToGroupCommandResult>();

        conversation.AddMemberToGroup(request.UserId);

        await _repository.SaveChangesAsync(cancellationToken);

        Member newMember = conversation.GetMemberByUserId(request.UserId);

        await _messageBus.SimplePublishAsync(new MemberAddedToGroupEvent(conversation.Id, newMember.Id, request.UserId), cancellationToken);

        return HandlerResult.Create(new AddMemberToGroupCommandResult(newMember.Id, newMember.ConversationId));
    }
}