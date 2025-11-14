using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed class GetConversationByIdQueryValidator : AbstractValidator<GetConversationByIdQuery>
{
    public GetConversationByIdQueryValidator()
    {
    }
}