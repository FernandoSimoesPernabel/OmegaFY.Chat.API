using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Infra.IA.Implementations.Base;
using OmegaFY.Chat.API.Infra.IA.Models;

namespace OmegaFY.Chat.API.Infra.IA.Implementations;

public sealed class SuggestReplyAgent : AgentBase<object, object>
{
    public SuggestReplyAgent(ILogger<AgentBase<object, object>> logger, IChatClient chatClient)
        : base(logger, chatClient) { }

    protected override string BuildSystemPrompt()
    {
        throw new NotImplementedException();
    }

    protected override string BuildUserPrompt(object request)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateRequest(object request)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateResult(object result)
    {
        throw new NotImplementedException();
    }

    protected override AgentOptions BuildAgentOptions()
    {
        return base.BuildAgentOptions() with
        {
            Temperature = 0.7f,
            MaxOutputTokens = 200
        };
    }
}