using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

public sealed class GetCurrentUserInfoQueryValidator : AbstractValidator<GetCurrentUserInfoQuery>
{
    public GetCurrentUserInfoQueryValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("A consulta não foi informada.");
    }
}