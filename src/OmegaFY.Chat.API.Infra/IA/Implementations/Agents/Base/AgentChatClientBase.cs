using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.IA.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Infra.IA.Implementations.Agents.Base;

public abstract class AgentChatClientBase<TRequest, TResult> : IAgent<TRequest, TResult> where TRequest : class where TResult : class
{
    private static readonly AgentOptions DEFAULT_AGENT_OPTIONS = new()
    {
        Model = AgentModel.Gemini_3_5_Flash_Lite,
        Temperature = 0,
        MaxOutputTokens = 1000
    };

    protected readonly ILogger<AgentChatClientBase<TRequest, TResult>> _logger;

    protected readonly IChatClient _chatClient;

    protected readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    protected readonly AgentOptions _agentOptions;

    protected AgentChatClientBase(
        ILogger<AgentChatClientBase<TRequest, TResult>> logger, 
        IServiceProvider serviceProvider, 
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider)
    {
        _logger = logger;
        _openTelemetryRegisterProvider = openTelemetryRegisterProvider;
        _agentOptions = BuildAgentOptions();

        _chatClient = serviceProvider.GetRequiredKeyedService<IChatClient>(_agentOptions.Provider);
    }

    protected abstract string BuildSystemPrompt();

    protected abstract string BuildUserPrompt(TRequest request);

    protected abstract void ValidateRequest(TRequest request);

    protected abstract void ValidateResult(TResult result);

    protected virtual AgentOptions BuildAgentOptions() => DEFAULT_AGENT_OPTIONS;

    public async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        
        TResult result = await GetChatResponseAsync(request, cancellationToken);

        ValidateResult(result);

        return result;
    }

    private async Task<TResult> GetChatResponseAsync(TRequest request, CancellationToken cancellationToken)
    {
        using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_AI_CHAT_COMPLETION_NAME);

        try
        {
            activity.SetAiAgentName(GetType().Name);
            activity.SetAiRequest(_agentOptions);

            ChatMessage[] messages = [new ChatMessage(ChatRole.System, BuildSystemPrompt()), new ChatMessage(ChatRole.User, BuildUserPrompt(request))];

            ChatResponse response = await _chatClient.GetResponseAsync<TResult>(messages, _agentOptions.ToChatOptions(), cancellationToken: cancellationToken);

            activity.SetAiResponse(response);
            activity.SetOkStatus();

            LogChatResponse(response);

            return string.IsNullOrWhiteSpace(response.Text) ? default : JsonSerializerHelper.Deserialize<TResult>(response.Text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while executing chat response for request: {Request}", request);

            activity.SetErrorStatus(ex);

            throw;
        }
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