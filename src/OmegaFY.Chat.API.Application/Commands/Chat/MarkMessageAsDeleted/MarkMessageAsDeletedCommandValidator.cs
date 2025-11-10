using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;

public sealed class MarkMessageAsDeletedCommandValidator : AbstractValidator<MarkMessageAsDeletedCommand>
{
    public MarkMessageAsDeletedCommandValidator()
    {
    }
}