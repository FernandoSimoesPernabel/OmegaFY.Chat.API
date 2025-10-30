using OmegaFY.Chat.API.Application.Commands.Chat.SendMessage;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.WebAPI.Models;
using OmegaFY.Chat.API.WebAPI.Models.Chat;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class ConversationsController : ApiControllerBase
{
    //[HttpPost()] //Creategroup

    //[HttpPut("{conversationId:guid}/GrupoConfig")]

    //[HttpPost("{conversationId:guid}/Members")]

    //[HttpDelete("{conversationId:guid}/Members/{memberId:guid}")]

    //[HttpGet("{conversationId:guid}/Messages/{messageId:guid}")]

    [HttpPost("{conversationId:guid}/Messages")]
    [ProducesResponseType(typeof(ApiResponse<SendMessageCommandResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage([FromServices] SendMessageCommandHandler handler, Guid conversationId, SendMessageRequest request, CancellationToken cancellationToken)
    {
        HandlerResult<SendMessageCommandResult> result = await handler.HandleAsync(request.ToCommand(conversationId), cancellationToken);
        return CreatedAtAction(nameof(SendMessage), new { result.Data?.MessageId }, result); //TODO : Change to GetMessageById action when implemented
    }
}