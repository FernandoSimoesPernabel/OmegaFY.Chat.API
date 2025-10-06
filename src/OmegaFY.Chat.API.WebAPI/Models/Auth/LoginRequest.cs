using OmegaFY.Chat.API.Application.Commands.Auth.Login;

namespace OmegaFY.Chat.API.WebAPI.Models.Auth;

public sealed record class LoginRequest
{
    public string Email { get; init; }

    public string Password { get; init; }

    public bool RememberMe { get; init; }

    public LoginCommand ToCommand() => new LoginCommand(Email, Password, RememberMe);
}