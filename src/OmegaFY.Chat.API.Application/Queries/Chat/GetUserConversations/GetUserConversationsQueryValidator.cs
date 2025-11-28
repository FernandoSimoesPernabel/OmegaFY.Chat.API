using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversations;

public sealed class GetUserConversationsQueryValidator : AbstractValidator<GetUserConversationsQuery>
{
    public GetUserConversationsQueryValidator()
    {
    }
}