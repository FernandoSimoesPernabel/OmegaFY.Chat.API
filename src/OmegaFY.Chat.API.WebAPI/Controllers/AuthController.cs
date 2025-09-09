using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.WebAPI.Models.Auth;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class AuthController : ApiControllerBase
{
    [HttpPost("/register-new-user")]
    public async Task<IActionResult> RegisterNewUser(
        [FromServices] RegisterNewUserCommandHandler handler,
        [FromBody] RegisterNewUserRequest request,
        CancellationToken cancellationToken)
    {
        return CreatedAtAction(nameof(UsersController.GetCurrentUserInfo), await handler.HandleAsync(request.ToCommand(), cancellationToken));
    }
}