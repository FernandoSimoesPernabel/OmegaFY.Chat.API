using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class ChangeGroupConfigCommandValidator : AbstractValidator<ChangeGroupConfigCommand>
{
    public ChangeGroupConfigCommandValidator()
    {
    }
}