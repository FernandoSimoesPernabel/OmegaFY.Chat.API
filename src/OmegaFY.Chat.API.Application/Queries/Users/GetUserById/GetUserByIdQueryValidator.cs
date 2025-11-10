using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("O ID do usuário não pode ser vazio.");   
    }
}