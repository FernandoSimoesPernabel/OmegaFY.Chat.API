using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;

public sealed class MarkMessageAsDeletedCommandValidator : AbstractValidator<MarkMessageAsDeletedCommand>
{
    public MarkMessageAsDeletedCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.MessageId).NotEmpty().WithMessage("O ID da mensagem é obrigatório.");
    }
}