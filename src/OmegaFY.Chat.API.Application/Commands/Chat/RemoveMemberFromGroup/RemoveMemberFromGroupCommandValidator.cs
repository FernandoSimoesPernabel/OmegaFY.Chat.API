using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;

public sealed class RemoveMemberFromGroupCommandValidator : AbstractValidator<RemoveMemberFromGroupCommand>
{
    public RemoveMemberFromGroupCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.MemberId).NotEmpty().WithMessage("O ID do membro é obrigatório.");
    }
}