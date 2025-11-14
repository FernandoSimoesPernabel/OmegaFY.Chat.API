using FluentValidation;
using Microsoft.Extensions.Options;
using OmegaFY.Chat.API.Domain.Constants;
using OmegaFY.Chat.API.Infra.Authentication.Models;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed class RegisterNewUserCommandValidator : AbstractValidator<RegisterNewUserCommand>
{
    public RegisterNewUserCommandValidator(IOptions<AuthenticationSettings> authenticationOptions)
    {
        AuthenticationSettings authenticationSettings = authenticationOptions.Value;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O Email informado está inválido.")
            .EmailAddress().WithMessage("O Email informado está inválido.")
            .MaximumLength(UserConstants.MAX_EMAIL_LENGTH).WithMessage("O Email informado está inválido.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Não foi informado um nome para o usuário")
            .Length(UserConstants.MIN_DISPLAY_NAME_LENGTH, UserConstants.MAX_DISPLAY_NAME_LENGTH)
            .WithMessage($"O nome de usuário deve ter entre {UserConstants.MIN_DISPLAY_NAME_LENGTH} e {UserConstants.MAX_DISPLAY_NAME_LENGTH}.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha não foi informada.")
            .Length(authenticationSettings.PasswordMinRequiredLength, authenticationSettings.PasswordMaxRequiredLength)
            .WithMessage($"A senha deve ter entre {authenticationSettings.PasswordMinRequiredLength} e {authenticationSettings.PasswordMaxRequiredLength} caracteres.")
            .Custom((password, context) =>
            {
                char[] passwordAsChars = password?.ToCharArray() ?? [];

                if (authenticationSettings.PasswordRequireDigit && !passwordAsChars.Any(c => char.IsDigit(c)))
                    context.AddFailure(nameof(authenticationSettings.PasswordRequireDigit), "A senha deve conter ao menos um dígito.");

                if (authenticationSettings.PasswordRequireLowercase && !passwordAsChars.Any(c => char.IsLower(c)))
                    context.AddFailure(nameof(authenticationSettings.PasswordRequireLowercase), "A senha deve conter ao menos uma letra minúscula.");

                if (authenticationSettings.PasswordRequireUppercase && !passwordAsChars.Any(c => char.IsUpper(c)))
                    context.AddFailure(nameof(authenticationSettings.PasswordRequireUppercase), "A senha deve conter ao menos uma letra maiúscula.");

                if (authenticationSettings.PasswordRequireNonAlphanumeric && !passwordAsChars.Any(c => !char.IsLetterOrDigit(c)))
                    context.AddFailure(nameof(authenticationSettings.PasswordRequireNonAlphanumeric), "A senha deve conter ao menos um caractere especial.");

                if (authenticationSettings.PasswordRequiredUniqueChars > 0)
                    if (passwordAsChars.Distinct().Count() < authenticationSettings.PasswordRequiredUniqueChars)
                        context.AddFailure(nameof(authenticationSettings.PasswordRequiredUniqueChars), $"A senha deve conter ao menos {authenticationSettings.PasswordRequiredUniqueChars} caracteres únicos.");
            });
    }
}