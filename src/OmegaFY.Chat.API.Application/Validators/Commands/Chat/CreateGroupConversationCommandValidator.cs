using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class CreateGroupConversationCommandValidator : AbstractValidator<CreateGroupConversationCommand>
{
    public CreateGroupConversationCommandValidator()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("O nome do grupo é obrigatório.")
            .MaximumLength(ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH).WithMessage($"O nome do grupo não pode exceder {ChatConstants.GROUP_CHAT_NAME_MAX_LENGTH} caracteres.");

        RuleFor(x => x.MaxNumberOfMembers)
            .InclusiveBetween(ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS, ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS)
            .WithMessage($"O número máximo de membros deve estar entre {ChatConstants.GROUP_CHAT_MIN_NUMBER_OF_MEMBERS} e {ChatConstants.GROUP_CHAT_MAX_NUMBER_OF_MEMBERS}.");
    }
}