using FluentValidation;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;

public sealed class ChangeGroupConfigCommandValidator : AbstractValidator<ChangeGroupConfigCommand>
{
    public ChangeGroupConfigCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.NewGroupName)
            .NotEmpty().WithMessage("O nome do grupo é obrigatório.")
            .MaximumLength(ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH).WithMessage($"O nome do grupo não pode exceder {ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH} caracteres.");

        RuleFor(x => x.NewMaxNumberOfMembers).InclusiveBetween(ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS, ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS)
            .WithMessage($"O número máximo de membros deve estar entre {ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS} e {ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS}.");
    }
}