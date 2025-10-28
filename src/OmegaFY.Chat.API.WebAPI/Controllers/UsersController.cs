using OmegaFY.Chat.API.Application.Commands.Users.AcceptFriendshipRequest;
using OmegaFY.Chat.API.Application.Commands.Users.RejectFriendshipRequest;
using OmegaFY.Chat.API.Application.Commands.Users.RemoveFriendship;
using OmegaFY.Chat.API.Application.Commands.Users.SendFriendshipRequest;
using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;
using OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;
using OmegaFY.Chat.API.Application.Queries.Users.GetUserById;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.WebAPI.Models;
using OmegaFY.Chat.API.WebAPI.Models.Users;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class UsersController : ApiControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<GetCurrentUserInfoQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserInfo([FromServices] GetCurrentUserInfoQueryHandler handler, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(new GetCurrentUserInfoQuery(), cancellationToken));

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetUserByIdQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromServices] GetUserByIdQueryResultHandler handler, [FromRoute] Guid userId, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(new GetUserByIdQuery(userId), cancellationToken));

    [HttpGet("me/friendships/{friendshipId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetFriendshipByIdQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFriendshipById([FromServices] GetFriendshipByIdQueryHandler handler, [FromRoute] Guid friendshipId, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(new GetFriendshipByIdQuery(friendshipId), cancellationToken));

    [HttpPost("me/friendships")]
    [ProducesResponseType(typeof(ApiResponse<SendFriendshipRequestCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendFriendshipRequest([FromServices] SendFriendshipRequestCommandHandler handler, [FromBody] SendFriendshipRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<SendFriendshipRequestCommandResult> result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return CreatedAtAction(nameof(GetFriendshipById), new { result.Data?.FriendshipId }, result);
    }

    [HttpPost("me/friendships/{friendshipId:guid}/accept")]
    [ProducesResponseType(typeof(ApiResponse<AcceptFriendshipRequestCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptFriendshipRequest([FromServices] AcceptFriendshipRequestCommandHandler handler, [FromRoute] Guid friendshipId, CancellationToken cancellationToken)
    {
        HandlerResult<AcceptFriendshipRequestCommandResult> result = await handler.HandleAsync(new AcceptFriendshipRequestCommand(friendshipId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }

    [HttpPost("me/friendships/{friendshipId:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<RejectFriendshipRequestCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectFriendshipRequest([FromServices] RejectFriendshipRequestCommandHandler handler, [FromRoute] Guid friendshipId, CancellationToken cancellationToken)
    {
        HandlerResult<RejectFriendshipRequestCommandResult> result = await handler.HandleAsync(new RejectFriendshipRequestCommand(friendshipId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }

    [HttpDelete("me/friendships/{friendshipId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RemoveFriendshipCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveFriendship([FromServices] RemoveFriendshipCommandHandler handler, [FromRoute] Guid friendshipId, CancellationToken cancellationToken)
    {
        HandlerResult<RemoveFriendshipCommandResult> result = await handler.HandleAsync(new RemoveFriendshipCommand(friendshipId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }
}