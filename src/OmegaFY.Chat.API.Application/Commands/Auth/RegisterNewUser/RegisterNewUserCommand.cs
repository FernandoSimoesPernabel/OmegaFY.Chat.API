namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed record class RegisterNewUserCommand : ICommand
{
    public string Email { get; init; }

    public string DisplayName { get; init; }

    public string Password { internal get; init; }

    public RegisterNewUserCommand() { }

    public RegisterNewUserCommand(string email, string displayName, string password)
    {
        Email = email;
        DisplayName = displayName;
        Password = password;
    }
}