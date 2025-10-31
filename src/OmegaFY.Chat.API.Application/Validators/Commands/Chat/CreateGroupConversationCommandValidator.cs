using FluentValidation;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;

namespace OmegaFY.Chat.API.Application.Validators.Commands.Chat;

public sealed class CreateGroupConversationCommandValidator : AbstractValidator<CreateGroupConversationCommand>
{
    public CreateGroupConversationCommandValidator()
    {
    }
}