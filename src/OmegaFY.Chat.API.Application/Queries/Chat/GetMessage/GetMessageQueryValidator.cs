using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessage;

public sealed class GetMessageQueryValidator : AbstractValidator<GetMessageQuery>
{
    public GetMessageQueryValidator()
    {
    }
}