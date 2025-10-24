using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Users;

public sealed class RemoveFriendshipCommandValidator : AbstractValidator<RemoveFriendshipCommand>
{
    public RemoveFriendshipCommandValidator()
    {
        RuleFor(x => x.FriendshipId).NotEmpty().WithMessage("O ID da amizade não pode ser vazio.");
    }
}