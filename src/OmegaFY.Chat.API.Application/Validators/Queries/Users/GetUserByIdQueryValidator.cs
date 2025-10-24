using FluentValidation;
using OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

namespace OmegaFY.Chat.API.Application.Validators.Queries.Users;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("O ID do usuário não pode ser vazio.");   
    }
}