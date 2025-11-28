using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed class GetMessageFromMemberQueryValidator : AbstractValidator<GetMessageFromMemberQuery>
{
    public GetMessageFromMemberQueryValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("O ID da conversa é obrigatório.");

        RuleFor(x => x.MessageId).NotEmpty().WithMessage("O ID da mensagem é obrigatório.");
    }
}