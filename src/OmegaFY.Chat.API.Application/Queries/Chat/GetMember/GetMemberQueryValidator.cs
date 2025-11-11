using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMember;

public sealed class GetMemberQueryValidator : AbstractValidator<GetMemberQuery>
{
    public GetMemberQueryValidator()
    {
    }
}