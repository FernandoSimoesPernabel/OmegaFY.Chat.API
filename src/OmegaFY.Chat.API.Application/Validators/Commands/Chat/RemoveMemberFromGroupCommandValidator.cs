using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class RemoveMemberFromGroupCommandValidator : AbstractValidator<RemoveMemberFromGroupCommand>
{
    public RemoveMemberFromGroupCommandValidator()
    {
    }
}