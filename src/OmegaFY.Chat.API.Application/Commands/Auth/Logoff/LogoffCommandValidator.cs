using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Auth.Logoff;

public sealed class LogoffCommandValidator : AbstractValidator<LogoffCommand>
{
    public LogoffCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("O Refresh Token não foi informado.");
    }
}