using OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.WebAPI.Models;
using OmegaFY.Chat.API.WebAPI.Models.Users;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<GetCurrentUserInfoQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUserInfo([FromServices] GetCurrentUserInfoQueryHandler handler, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(new GetCurrentUserInfoQuery(), cancellationToken));

    [HttpGet("me/friendships/{friendshipId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetFriendshipByIdQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task <IActionResult> GetFriendshipById([FromServices] GetFriendshipByIdQueryHandler handler, [FromRoute] Guid friendshipId, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(new GetFriendshipByIdQuery(friendshipId), cancellationToken));

    [HttpPost("me/friendships")]
    [ProducesResponseType(typeof(ApiResponse<SendFriendshipRequestCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendFriendshipRequest([FromServices] SendFriendshipRequestCommandHandler handler, SendFriendshipRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<SendFriendshipRequestCommandResult> result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return CreatedAtAction(nameof(GetFriendshipById), new { result.Data?.FriendshipId });
    }
}