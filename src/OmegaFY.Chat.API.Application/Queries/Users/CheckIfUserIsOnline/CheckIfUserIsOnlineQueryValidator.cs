using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Users.CheckIfUserIsOnline;

public sealed class CheckIfUserIsOnlineQueryValidator : AbstractValidator<CheckIfUserIsOnlineQuery>
{
    public CheckIfUserIsOnlineQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("O ID do usuário não pode ser vazio.");
    }
}