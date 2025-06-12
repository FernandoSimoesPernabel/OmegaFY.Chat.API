using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.WebAPI.DTOs.Auth;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class AuthController : ApiControllerBase
{
    public AuthController(IServiceBus serviceBus) : base(serviceBus) { }

    [HttpPost("/register-new-user")]
    public async Task<RegisterNewUserCommandResult> RegisterNewUser(RegisterNewUserRequest request, CancellationToken cancellationToken)
        => await _serviceBus.SendMessageAsync<RegisterNewUserCommand, RegisterNewUserCommandResult>(request.ToCommand(), cancellationToken);
}