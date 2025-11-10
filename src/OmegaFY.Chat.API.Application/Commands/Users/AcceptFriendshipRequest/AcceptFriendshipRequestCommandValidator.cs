using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;

public sealed class AcceptFriendshipRequestCommandValidator : AbstractValidator<AcceptFriendshipRequestCommand>
{
    public AcceptFriendshipRequestCommandValidator()
    {
        RuleFor(x => x.FriendshipId).NotEmpty().WithMessage("O ID da amizade não pode ser vazio.");
    }
}