using OmegaFY.Chat.API.Application.Commands.Chat.AddMemberToGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsDeleted;
using OmegaFY.Chat.API.Application.Commands.Chat.MarkMessageAsRead;
using OmegaFY.Chat.API.Application.Commands.Chat.RemoveMemberFromGroup;
using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.WebAPI.Models;
using OmegaFY.Chat.API.WebAPI.Models.Chat;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class ConversationsController : ApiControllerBase
{
    //[HttpGet("{conversationId:guid}")]

    //[HttpGet("{conversationId:guid}/Members/{memberId:guid}")]

    //[HttpGet("{conversationId:guid}/Messages/{messageId:guid}")]

    [HttpPost()]
    [ProducesResponseType(typeof(ApiResponse<CreateGroupConversationCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGroupConversation(CreateGroupConversationCommandHandler handler, [FromBody] CreateGroupConversationRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<CreateGroupConversationCommandResult> result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return CreatedAtAction(nameof(CreateGroupConversation), new { result.Data?.ConversationId }, result); //TODO : Change to GetConversationById action when implemented
    }

    [HttpPost("{conversationId:guid}/Messages")]
    [ProducesResponseType(typeof(ApiResponse<SendMessageCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(SendMessageCommandHandler handler, Guid conversationId, [FromBody] SendMessageRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<SendMessageCommandResult> result = await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken);
        return CreatedAtAction(nameof(SendMessage), new { result.Data?.MessageId }, result); //TODO : Change to GetMessageById action when implemented
    }

    [HttpPost("{conversationId:guid}/Messages/{messageId:guid}/mark-as-read")]
    [ProducesResponseType(typeof(ApiResponse<MarkMessageAsReadCommandResult>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkMessageAsRead(MarkMessageAsReadCommandHandler handler, [FromRoute] Guid conversationId, [FromRoute] Guid messageId, CancellationToken cancellationToken)
    {
        HandlerResult<MarkMessageAsReadCommandResult> result = await handler.HandleAsync(new MarkMessageAsReadCommand(conversationId, messageId), cancellationToken);
        return result.Succeeded() ? NoContent() : BadRequest(result);
    }

    [HttpPost("{conversationId:guid}/Members")]
    [ProducesResponseType(typeof(ApiResponse<AddMemberToGroupCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddMemberToGroup(AddMemberToGroupCommandHandler handler, Guid conversationId, [FromBody] AddMemberToGroupRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<AddMemberToGroupCommandResult> result = await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken);
        return CreatedAtAction(nameof(AddMemberToGroup), new { result.Data?.MemberId }, result); //TODO : Change to GetMemberById action when implemented
    }

    [HttpPut("{conversationId:guid}/GrupoConfig")]
    [ProducesResponseType(typeof(ApiResponse<ChangeGroupConfigCommandResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeGroupConfig(ChangeGroupConfigCommandHandler handler, [FromRoute] Guid conversationId, [FromBody] ChangeGroupConfigRequest request, CancellationToken cancellationToken)
        => Ok(await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken));

    [HttpDelete("{conversationId:guid}/Members/{memberId:guid}")]
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

    [HttpDelete("{conversationId:guid}/Messages/{messageId:guid}")]
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