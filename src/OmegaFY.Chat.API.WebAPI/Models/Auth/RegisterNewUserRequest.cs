using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Shared;

namespace OmegaFY.Chat.API.WebAPI.Models.Auth;

public sealed record class RegisterNewUserRequest : IRequest
{
    public string Email { get; init; }

    public string DisplayName { get; init; }

    public string Password { internal get; init; }

    public RegisterNewUserCommand ToCommand() => new RegisterNewUserCommand(Email, DisplayName, Password);
}