using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;

public sealed class GetMemberFromConversationQueryValidator : AbstractValidator<GetMemberFromConversationQuery>
{
    public GetMemberFromConversationQueryValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.MemberId).NotEmpty().WithMessage("O ID do membro é obrigatório.");
    }
}