using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed class GetUserConversationMessagesQueryValidator : AbstractValidator<GetUserConversationMessagesQuery>
{
    public GetUserConversationMessagesQueryValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.Pagination).NotNull().WithMessage("É obrigatório fornecer as informações de paginação.");

        RuleFor(x => x.Pagination.Take).GreaterThan(0).WithMessage("O número de itens a retornar deve ser maior que zero.");
    }
}