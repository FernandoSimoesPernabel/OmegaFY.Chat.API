using FluentValidation;
using OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;

namespace OmegaFY.Chat.API.Application.Validators.Queries.Users;

public sealed class GetFriendshipByIdQueryValidator : AbstractValidator<GetFriendshipByIdQuery>
{
    public GetFriendshipByIdQueryValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("A consulta não foi informada.");
    }
}