using Microsoft.Extensions.AI;
using OmegaFY.Chat.API.Infra.IA.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class AgentOptionsExtensions
{
    public static ChatOptions ToChatOptions(this AgentOptions options)
    {
        return new ChatOptions()
        {
            ModelId = options.ModelId,
            Temperature = options.Temperature,
            MaxOutputTokens = options.MaxOutputTokens
        };
    }
}