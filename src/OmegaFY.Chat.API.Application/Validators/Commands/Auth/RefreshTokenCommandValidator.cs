using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Auth;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.CurrentToken).NotEmpty().WithMessage("O Token atual não foi informado.");

        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("O Refresh Token não foi informado.");
    }
}