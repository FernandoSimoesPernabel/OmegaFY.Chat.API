using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.Infra.IA.Models;

public sealed record class AgentOptions
{
    public AgentModel Model { get; set; }
    
    public AgentModelProvider Provider => Model.ToModelProvider();
    
    public string ModelId => Model.ToModelId();

    public float? Temperature { get; set; }

    public int? MaxOutputTokens { get; set; }
}