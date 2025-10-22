using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Users;

public sealed class AcceptFriendshipRequestCommandValidator : AbstractValidator<AcceptFriendshipRequestCommand>
{
    public AcceptFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.FriendshipId).NotEmpty().WithMessage("O ID da amizade não pode ser vazio.");
    }
}