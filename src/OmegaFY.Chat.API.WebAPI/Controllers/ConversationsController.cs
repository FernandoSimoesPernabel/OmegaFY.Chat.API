using OmegaFY.Chat.API.Application.Commands.Chat.ChangeGroupConfig;
using OmegaFY.Chat.API.Application.Commands.Chat.CreateGroupConversation;
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
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage(SendMessageCommandHandler handler, Guid conversationId, [FromBody] SendMessageRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<SendMessageCommandResult> result = await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken);
        return CreatedAtAction(nameof(SendMessage), new { result.Data?.MessageId }, result); //TODO : Change to GetMessageById action when implemented
    }

    [HttpPut("{conversationId:guid}/GrupoConfig")]
    [ProducesResponseType(typeof(ApiResponse<ChangeGroupConfigCommandResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeGroupConfig(ChangeGroupConfigCommandHandler handler, [FromRoute] Guid conversationId, [FromBody] ChangeGroupConfigRequest request, CancellationToken cancellationToken) 
        => Ok(await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken));

    //[HttpPost("{conversationId:guid}/Members")]

    //[HttpDelete("{conversationId:guid}/Members/{memberId:guid}")]
}