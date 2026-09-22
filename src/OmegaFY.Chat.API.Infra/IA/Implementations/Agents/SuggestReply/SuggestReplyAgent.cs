using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Infra.IA.Implementations.Agents.Base;
using OmegaFY.Chat.API.Infra.IA.Models;

namespace OmegaFY.Chat.API.Infra.IA.Implementations.Agents.SuggestReply;

public sealed class SuggestReplyAgent : AgentBase<SuggestReplyRequest, SuggestReplyResult>
{
    public SuggestReplyAgent(ILogger<AgentBase<SuggestReplyRequest, SuggestReplyResult>> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider) { }

    protected override string BuildSystemPrompt()
    {
        throw new NotImplementedException();
    }

    protected override string BuildUserPrompt(SuggestReplyRequest request)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateRequest(SuggestReplyRequest request)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateResult(SuggestReplyResult result)
    {
        throw new NotImplementedException();
    }

    protected override AgentOptions BuildAgentOptions()
    {
        return base.BuildAgentOptions() with
        {
            Model = AgentModel.Gemini_3_8_Flash,
            Temperature = 0.7f,
            MaxOutputTokens = 200
        };
    }
}