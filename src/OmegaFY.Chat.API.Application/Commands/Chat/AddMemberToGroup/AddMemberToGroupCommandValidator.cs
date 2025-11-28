using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed class AddMemberToGroupCommandValidator : AbstractValidator<AddMemberToGroupCommand>
{
    public AddMemberToGroupCommandValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("O ID do usuário é obrigatório.");
    }
}