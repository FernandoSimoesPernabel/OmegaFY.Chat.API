using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.WebAPI.DTOs.Auth;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class AuthController : ApiControllerBase
{
    [HttpPost("/register-new-user")]
    public async Task<RegisterNewUserCommandResult> RegisterNewUser(
        [FromServices] RegisterNewUserCommandHandler handler,
        [FromBody] RegisterNewUserRequest request,
        CancellationToken cancellationToken) => await handler.HandleAsync(request.ToCommand(), cancellationToken);
}