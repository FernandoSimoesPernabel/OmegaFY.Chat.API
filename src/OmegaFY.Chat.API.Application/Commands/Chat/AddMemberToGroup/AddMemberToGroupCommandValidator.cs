using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

public sealed class AddMemberToGroupCommandValidator : AbstractValidator<AddMemberToGroupCommand>
{
    public AddMemberToGroupCommandValidator()
    {
    }
}