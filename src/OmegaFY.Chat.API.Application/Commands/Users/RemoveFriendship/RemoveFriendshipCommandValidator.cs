using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;

public sealed class RemoveFriendshipCommandValidator : AbstractValidator<RemoveFriendshipCommand>
{
    public RemoveFriendshipCommandValidator()
    {
        RuleFor(x => x.FriendshipId).NotEmpty().WithMessage("O ID da amizade não pode ser vazio.");
    }
}