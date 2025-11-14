using Microsoft.AspNetCore.Authorization;
using OmegaFY.Chat.API.Application.Commands.Auth.Login;
using OmegaFY.Chat.API.Application.Commands.Auth.Logoff;
using OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;
using OmegaFY.Chat.API.WebAPI.Models;
using OmegaFY.Chat.API.WebAPI.Models.Auth;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class AuthController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("register-new-user")]
    [ProducesResponseType(typeof(ApiResponse<RegisterNewUserCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser(RegisterNewUserCommandHandler handler, [FromBody] RegisterNewUserRequest request, CancellationToken cancellationToken)
        => Created(Url.ActionLink(nameof(UsersController.GetCurrentUserInfo), "Users"), await handler.HandleAsync(request.ToCommand(), cancellationToken));

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginCommandResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginCommandHandler handler, [FromBody] LoginRequest request, CancellationToken cancellationToken) 
        => Ok(await handler.HandleAsync(request.ToCommand(), cancellationToken));

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<RefreshTokenCommandResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommandHandler handler, [FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(request.ToCommand(), cancellationToken));

    [HttpDelete("logoff")]
    [ProducesResponseType(typeof(ApiResponse<LogoffCommandResult>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logoff(LogoffCommandHandler handler, [FromBody] LogoffRequest request, CancellationToken cancellationToken)
        => Accepted(await handler.HandleAsync(request.ToCommand(), cancellationToken));
}