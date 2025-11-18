using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed class GetConversationByIdQueryValidator : AbstractValidator<GetConversationByIdQuery>
{
    public GetConversationByIdQueryValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O Id da conversa é obrigatório.");
    }
}