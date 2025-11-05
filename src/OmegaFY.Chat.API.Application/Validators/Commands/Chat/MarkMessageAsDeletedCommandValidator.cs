using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class MarkMessageAsDeletedCommandValidator : AbstractValidator<MarkMessageAsDeletedCommand>
{
    public MarkMessageAsDeletedCommandValidator()
    {
    }
}