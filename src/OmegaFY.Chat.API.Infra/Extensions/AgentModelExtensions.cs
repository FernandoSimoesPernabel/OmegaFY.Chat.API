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
            AgentModel.Gemini_3_7_Flash => "gemini-3.7-flash",
            AgentModel.Gemini_3_8_Flash => "gemini-3.8-flash",
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, "Unknown model")
        };
    }
}