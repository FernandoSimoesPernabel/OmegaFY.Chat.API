using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;

public sealed class MarkMessageAsReadCommandValidator : AbstractValidator<MarkMessageAsReadCommand>
{
    public MarkMessageAsReadCommandValidator()
    {
    }
}