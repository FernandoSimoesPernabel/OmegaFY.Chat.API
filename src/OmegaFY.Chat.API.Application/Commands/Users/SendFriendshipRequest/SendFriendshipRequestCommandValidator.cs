using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;

public sealed class SendFriendshipRequestCommandValidator : AbstractValidator<SendFriendshipRequestCommand>
{
    public SendFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.InvitedUserId).NotEmpty().WithMessage("O ID do usuário convidado não foi informado.");
    }
}