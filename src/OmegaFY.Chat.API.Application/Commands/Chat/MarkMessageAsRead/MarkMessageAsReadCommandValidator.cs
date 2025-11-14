using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;

public sealed class MarkMessageAsReadCommandValidator : AbstractValidator<MarkMessageAsReadCommand>
{
    public MarkMessageAsReadCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.MessageId).NotEmpty().WithMessage("O ID da mensagem é obrigatório.");
    }
}