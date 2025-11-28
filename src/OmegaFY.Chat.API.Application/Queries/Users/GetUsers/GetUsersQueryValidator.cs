using FluentValidation;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("A consulta não foi informada.");

        When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () =>
        {
            RuleFor(x => x.DisplayName)
                .MinimumLength(UserConstants.MIN_DISPLAY_NAME_LENGTH).WithMessage($"O nome de usuário deve conter no mínimo {UserConstants.MIN_DISPLAY_NAME_LENGTH} caracteres.")
                .MaximumLength(UserConstants.MAX_DISPLAY_NAME_LENGTH).WithMessage($"O nome de usuário deve conter no máximo {UserConstants.MAX_DISPLAY_NAME_LENGTH} caracteres.");
        });

        When(x => x.Status.HasValue, () =>
        {
            RuleFor(x => x.Status).IsInEnum().WithMessage("O status da amizade informado é inválido.");
        });
    }
}