using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.WebAPI.Models.Auth;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserInfo(
        [FromServices] GetCurrentUserInfoQueryHandler handler,
        GetCurrentUserInfoRequest request,
        CancellationToken cancellationToken) => Ok(await handler.HandleAsync(request.ToQuery(), cancellationToken));
}