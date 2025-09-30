using Microsoft.AspNetCore.Authorization;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.WebAPI.Models.Auth;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class AuthController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("register-new-user")]
    public async Task<IActionResult> RegisterNewUser(
        [FromServices] RegisterNewUserCommandHandler handler,
        [FromBody] RegisterNewUserRequest request,
        CancellationToken cancellationToken)
    {
        return Created(Url.ActionLink(nameof(UsersController.GetCurrentUserInfo), "Users"), await handler.HandleAsync(request.ToCommand(), cancellationToken));
    }
}