using OmegaFY.Chat.API.Infra.IA.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class AgentModelExtensions
{
    public static string ToModelId(this AgentModel model)
    {
        return model switch
        {
            AgentModel.Gemini_3_5_Flash_Lite => "gemini-3.5-flash-lite",
            AgentModel.Gemini_3_6_Flash => "gemini-3.6-flash",
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, "Unknown model")
        };
    }

    public static AgentModelProvider ToModelProvider(this AgentModel model)
    {
        return model switch
        {
            AgentModel.Gemini_3_5_Flash_Lite or AgentModel.Gemini_3_6_Flash => AgentModelProvider.Google,
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, "Unknown model")
        };
    }
}