using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.IA.Models;

namespace OmegaFY.Chat.API.Infra.IA.Implementations.Agents.Base;

public abstract class AgentBase<TRequest, TResult> : IAgent<TRequest, TResult> where TRequest : class where TResult : class
{
    private static readonly AgentOptions DEFAULT_AGENT_OPTIONS = new()
    {
        Model = AgentModel.Gemini_3_5_Flash_Lite,
        Temperature = 0,
        MaxOutputTokens = 1000
    };

    protected readonly ILogger<AgentBase<TRequest, TResult>> _logger;

    protected readonly IChatClient _chatClient;

    protected readonly AgentOptions _agentOptions;

    protected AgentBase(ILogger<AgentBase<TRequest, TResult>> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _agentOptions = BuildAgentOptions();
        _chatClient = serviceProvider.GetRequiredKeyedService<IChatClient>(_agentOptions.Model);
    }

    protected abstract string BuildSystemPrompt();

    protected abstract string BuildUserPrompt(TRequest request);

    protected abstract void ValidateRequest(TRequest request);

    protected abstract void ValidateResult(TResult result);

    protected virtual AgentOptions BuildAgentOptions() => DEFAULT_AGENT_OPTIONS;

    public async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        TResult result = await GetChatResponseAync(request, cancellationToken);

        ValidateResult(result);

        return result;
    }

    private async Task<TResult> GetChatResponseAync(TRequest request, CancellationToken cancellationToken)
    {
        ChatMessage[] messages = [new ChatMessage(ChatRole.System, BuildSystemPrompt()), new ChatMessage(ChatRole.User, BuildUserPrompt(request))];

        ChatResponse response = await _chatClient.GetResponseAsync(messages, _agentOptions.ToChatOptions(), cancellationToken);

        LogChatResponse(response);

        return JsonSerializerHelper.Deserialize<TResult>(response.Text);
    }

    private void LogChatResponse(ChatResponse response)
    {
        _logger.LogInformation(
            "Chat response received: CreatedAt={CreatedAt} ResponseId={ResponseId}, ConversationId={ConversationId}, ModelId={ModelId}, MessagesCount={MessagesCount}, FinishReason={FinishReason}",
            response.CreatedAt ?? DateTimeOffset.UtcNow,
            response.ResponseId,
            response.ConversationId,
            response.ModelId,
            response.Messages?.Count ?? 0,
            response.FinishReason);

        if (response.Usage is not null)
        {
            _logger.LogInformation(
                "Chat response usage: InputTokenCount={InputTokenCount}, OutputTokenCount={OutputTokenCount}, TotalTokenCount={TotalTokenCount}",
                response.Usage.InputTokenCount,
                response.Usage.OutputTokenCount,
                response.Usage.TotalTokenCount);
        }

        if (response.AdditionalProperties is not null && response.AdditionalProperties.Count > 0)
            _logger.LogInformation("Chat response additional properties count: {AdditionalPropertiesCount}", response.AdditionalProperties.Count);

        if (response.RawRepresentation is not null)
            _logger.LogInformation("Chat response raw representation type: {RawRepresentationType}", response.RawRepresentation.GetType().Name);
    }
}