using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Users;

public sealed class RejectFriendshipRequestCommandValidator : AbstractValidator<RejectFriendshipRequestCommand>
{
    public RejectFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.FriendshipId).NotEmpty().WithMessage("O ID da amizade não pode ser vazio.");
    }
}