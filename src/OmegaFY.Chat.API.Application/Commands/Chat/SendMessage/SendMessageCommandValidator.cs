using FluentValidation;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;

public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("O corpo da mensagem é obrigatório.")
            .MaximumLength(ChatConstants.MESSAGE_BODY_MAX_LENGTH).WithMessage($"O corpo da mensagem não pode exceder {ChatConstants.MESSAGE_BODY_MAX_LENGTH} caracteres.");
    }
}