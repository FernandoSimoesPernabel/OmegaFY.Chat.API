using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed class GetMessageFromMemberQueryValidator : AbstractValidator<GetMessageFromMemberQuery>
{
    public GetMessageFromMemberQueryValidator()
    {
    }
}