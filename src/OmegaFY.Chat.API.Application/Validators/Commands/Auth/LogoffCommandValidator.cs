using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Auth.Logoff;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Auth;

public sealed class LogoffCommandValidator : AbstractValidator<LogoffCommand>
{
    public LogoffCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("O Refresh Token não foi informado.");
    }
}