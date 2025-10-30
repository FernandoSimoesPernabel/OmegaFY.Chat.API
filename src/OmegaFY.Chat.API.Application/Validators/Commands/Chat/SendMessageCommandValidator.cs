using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa � obrigat�rio.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("O corpo da mensagem � obrigat�rio.")
            .MaximumLength(ChatConstants.MESSAGE_BODY_MAX_LENGTH).WithMessage($"O corpo da mensagem n�o pode exceder {ChatConstants.MESSAGE_BODY_MAX_LENGTH} caracteres.");
    }
}