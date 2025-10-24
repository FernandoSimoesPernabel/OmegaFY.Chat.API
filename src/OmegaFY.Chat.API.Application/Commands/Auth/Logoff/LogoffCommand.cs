namespace OmegaFY.Chat.API.Application.Commands.Auth.Logoff;

public sealed record class LogoffCommand : ICommand
{
    public string RefreshToken { get; init; }

    public LogoffCommand() { }

    public LogoffCommand(string refreshToken) => RefreshToken = refreshToken;
}