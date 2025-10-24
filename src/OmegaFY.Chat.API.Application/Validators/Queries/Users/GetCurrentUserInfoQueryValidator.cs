using FluentValidation;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

namespace OmegaFY.Chat.API.Application.Validators.Queries.Users;

public sealed class GetCurrentUserInfoQueryValidator : AbstractValidator<GetCurrentUserInfoQuery>
{
    public GetCurrentUserInfoQueryValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("A consulta não foi informada.");
    }
}