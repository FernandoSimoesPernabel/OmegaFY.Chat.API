using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class AddMemberToGroupCommandValidator : AbstractValidator<AddMemberToGroupCommand>
{
    public AddMemberToGroupCommandValidator()
    {
    }
}