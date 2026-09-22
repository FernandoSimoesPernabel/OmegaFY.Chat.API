using OmegaFY.Chat.API.Infra.IA.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class AgentModelExtensions
{
    public static string ToModelId(this AgentModel model)
    {
        return model switch
        {
            AgentModel.Gemini_1_5_Turbo => "gemini-1.5-turbo",
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}