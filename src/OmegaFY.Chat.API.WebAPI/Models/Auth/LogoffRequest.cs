using OmegaFY.Chat.API.Application.Commands.Auth.Logoff;

namespace OmegaFY.Chat.API.WebAPI.Models.Auth;

public sealed record class LogoffRequest
{
    public string RefreshToken { get; init; }

    public LogoffCommand ToCommand() => new LogoffCommand(RefreshToken);
}