using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Auth;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O Email informado está inválido.")
            .EmailAddress().WithMessage("O Email informado está inválido.")
            .MaximumLength(UserConstants.MAX_EMAIL_LENGTH).WithMessage("O Email informado está inválido.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("A senha não foi informada.");
    }
}