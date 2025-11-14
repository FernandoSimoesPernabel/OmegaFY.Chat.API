using FluentValidation;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;

public sealed class GetFriendshipByIdQueryValidator : AbstractValidator<GetFriendshipByIdQuery>
{
    public GetFriendshipByIdQueryValidator()
    {
        RuleFor(x => x.FriendshipId).NotEmpty().WithMessage("O ID da amizade não pode ser vazio.");
    }
}