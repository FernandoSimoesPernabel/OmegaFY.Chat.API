using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

namespace OmegaFY.Chat.API.WebAPI.DTOs.Auth;

public sealed record class RegisterNewUserRequest
{
    public string Email { get; init; }

    public string DisplayName { get; init; }

    public string Password { internal get; init; }

    public RegisterNewUserCommand ToCommand() => new RegisterNewUserCommand(Email, DisplayName, Password);
}