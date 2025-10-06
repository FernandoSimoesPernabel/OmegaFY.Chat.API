namespace OmegaFY.Chat.API.Application.Commands.Auth.Login;

public sealed record class LoginCommand : ICommand
{
    public string Email { get; init; }

    public string Password { internal get; init; }

    public bool RememberMe { get; init; }

    public LoginCommand() { }

    public LoginCommand(string email, string password, bool rememberMe)
    {
        Email = email;
        Password = password;
        RememberMe = rememberMe;
    }
}