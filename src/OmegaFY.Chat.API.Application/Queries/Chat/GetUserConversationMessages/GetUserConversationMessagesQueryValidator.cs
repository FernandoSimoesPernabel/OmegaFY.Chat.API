using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed class GetUserConversationMessagesQueryValidator : AbstractValidator<GetUserConversationMessagesQuery>
{
    public GetUserConversationMessagesQueryValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");
    }
}