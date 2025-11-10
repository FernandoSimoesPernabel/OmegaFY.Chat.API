using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

public sealed class ChangeGroupConfigCommandValidator : AbstractValidator<ChangeGroupConfigCommand>
{
    public ChangeGroupConfigCommandValidator()
    {
    }
}