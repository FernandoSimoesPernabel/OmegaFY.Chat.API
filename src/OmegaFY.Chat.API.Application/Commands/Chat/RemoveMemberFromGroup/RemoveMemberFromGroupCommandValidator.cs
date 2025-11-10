using FluentValidation;

namespace OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;

public sealed class RemoveMemberFromGroupCommandValidator : AbstractValidator<RemoveMemberFromGroupCommand>
{
    public RemoveMemberFromGroupCommandValidator()
    {
    }
}