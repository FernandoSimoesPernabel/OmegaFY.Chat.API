namespace OmegaFY.Chat.API.Application.Models;

public sealed record class GroupConfigModel
{
    public Guid GroupConfigId { get; init; }

    public Guid ConversationId { get; init; }
    
    public Guid CreatedByUserId { get; init; }
    
    public string GroupName { get; init; }
    
    public byte MaxNumberOfMembers { get; init; }
}