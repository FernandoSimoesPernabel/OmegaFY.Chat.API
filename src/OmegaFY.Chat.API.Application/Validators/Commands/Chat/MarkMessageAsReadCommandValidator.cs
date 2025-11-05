using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class MarkMessageAsReadCommandValidator : AbstractValidator<MarkMessageAsReadCommand>
{
    public MarkMessageAsReadCommandValidator()
    {
    }
}