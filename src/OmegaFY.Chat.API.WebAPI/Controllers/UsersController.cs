using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.WebAPI.Models;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<GetCurrentUserInfoQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUserInfo([FromServices] GetCurrentUserInfoQueryHandler handler, CancellationToken cancellationToken) 
        => Ok(await handler.HandleAsync(new GetCurrentUserInfoQuery(), cancellationToken));
}