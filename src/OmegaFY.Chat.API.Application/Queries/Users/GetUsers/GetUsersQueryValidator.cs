using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("A consulta não foi informada.");
    }
}