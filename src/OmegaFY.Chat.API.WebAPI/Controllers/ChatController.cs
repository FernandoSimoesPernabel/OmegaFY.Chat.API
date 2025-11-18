using OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;
using OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;
using OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;
using OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.WebAPI.Models;
using OmegaFY.Chat.API.WebAPI.Models.Chat;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class ChatController : ApiControllerBase
{
    //[HttpGet("me/get-unread-messages")]

    //[HttpGet("me/conversations")]

    //[HttpGet("me/{conversationId:guid}/messages/{messageId:guid}")]

    [HttpGet("{conversationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetConversationByIdQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversationById(GetConversationByIdQueryHandler handler, [FromRoute] Guid conversationId, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(new GetConversationByIdQuery(conversationId), cancellationToken));

    [HttpGet("{conversationId:guid}/members/{memberId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetMemberFromConversationQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMemberFromConversation(GetMemberFromConversationQueryHandler handler, [FromRoute] Guid conversationId, [FromRoute] Guid memberId, CancellationToken cancellationToken) 
        => Ok(await handler.HandleAsync(new GetMemberFromConversationQuery(conversationId, memberId), cancellationToken));

    [HttpGet("{conversationId:guid}/messages/{messageId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetMessageFromMemberQueryResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageFromMember(GetMessageFromMemberQueryHandler handler, [FromRoute] Guid conversationId, [FromRoute] Guid messageId, CancellationToken cancellationToken) 
        => Ok(await handler.HandleAsync(new GetMessageFromMemberQuery(conversationId, messageId), cancellationToken));

    [HttpPost()]
    [ProducesResponseType(typeof(ApiResponse<CreateGroupConversationCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGroupConversation(CreateGroupConversationCommandHandler handler, [FromBody] CreateGroupConversationRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<CreateGroupConversationCommandResult> result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return CreatedAtAction(nameof(GetConversationById), new { result.Data?.ConversationId }, result);
    }

    [HttpPost("{conversationId:guid}/messages")]
    [ProducesResponseType(typeof(ApiResponse<SendMessageCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(SendMessageCommandHandler handler, Guid conversationId, [FromBody] SendMessageRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<SendMessageCommandResult> result = await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken);
        return CreatedAtAction(nameof(GetMemberFromConversation), new { result.Data?.MessageId }, result);
    }

    [HttpPost("{conversationId:guid}/messages/{messageId:guid}/mark-as-read")]
    [ProducesResponseType(typeof(ApiResponse<MarkMessageAsReadCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkMessageAsRead(MarkMessageAsReadCommandHandler handler, [FromRoute] Guid conversationId, [FromRoute] Guid messageId, CancellationToken cancellationToken)
    {
        HandlerResult<MarkMessageAsReadCommandResult> result = await handler.HandleAsync(new MarkMessageAsReadCommand(conversationId, messageId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }

    [HttpPost("{conversationId:guid}/members")]
    [ProducesResponseType(typeof(ApiResponse<AddMemberToGroupCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMemberToGroup(AddMemberToGroupCommandHandler handler, Guid conversationId, [FromBody] AddMemberToGroupRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<AddMemberToGroupCommandResult> result = await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken);
        return CreatedAtAction(nameof(GetMessageFromMember), new { result.Data?.MemberId }, result);
    }

    [HttpPut("{conversationId:guid}/grupo-config")]
    [ProducesResponseType(typeof(ApiResponse<ChangeGroupConfigCommandResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeGroupConfig(ChangeGroupConfigCommandHandler handler, [FromRoute] Guid conversationId, [FromBody] ChangeGroupConfigRequest request, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken));

    [HttpDelete("{conversationId:guid}/members/{memberId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RemoveMemberFromGroupCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkMessageAsDeleted(RemoveMemberFromGroupCommandHandler handler, [FromRoute] Guid conversationId, [FromRoute] Guid memberId, CancellationToken cancellationToken)
    {
        HandlerResult<RemoveMemberFromGroupCommandResult> result = await handler.HandleAsync(new RemoveMemberFromGroupCommand(conversationId, memberId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }

    [HttpDelete("{conversationId:guid}/messages/{messageId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MarkMessageAsDeletedCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkMessageAsDeleted(MarkMessageAsDeletedCommandHandler handler, [FromRoute] Guid conversationId, [FromRoute] Guid messageId, CancellationToken cancellationToken)
    {
        HandlerResult<MarkMessageAsDeletedCommandResult> result = await handler.HandleAsync(new MarkMessageAsDeletedCommand(conversationId, messageId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }
}