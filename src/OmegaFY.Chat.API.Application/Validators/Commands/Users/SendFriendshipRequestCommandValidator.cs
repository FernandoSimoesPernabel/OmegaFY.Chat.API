using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Users;

public sealed class SendFriendshipRequestCommandValidator : AbstractValidator<SendFriendshipRequestCommand>
{
    public SendFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.InvitedUserId).NotEmpty().WithMessage("O ID do usuário convidado não foi informado.");
    }
}