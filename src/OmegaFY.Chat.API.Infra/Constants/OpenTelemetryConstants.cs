namespace OmegaFY.Chat.API.Infra.Constants;

public static class OpenTelemetryConstants
{
    public const string ACTIVITY_APPLICATION_HANDLER_NAME = "ApplicationHandler";

    public const string ACTIVITY_EVENT_HANDLER_NAME = "EventHandler";

    public const string ACTIVITY_CHAT_EVENTS_QUEUE_CONSUMER_NAME = "ChatEventsQueueConsumer";

    public const string ACTIVITY_HYBRID_CACHE_PROVIDER_NAME = "HybridCacheProvider";

    public const string ACTIVITY_AI_CHAT_COMPLETION_NAME = "AIAgentChatCompletion";

    public const string ACTIVITY_BASE_NAME = "omegafy";

    public const string REQUEST_CONTENT_KEY = $"{ACTIVITY_BASE_NAME}.request_content";

    public const string RESULT_CONTENT_KEY = $"{ACTIVITY_BASE_NAME}.result_content";

    public const string HANDLER_NAME_KEY = $"{ACTIVITY_BASE_NAME}.handler_name";

    public const string MESSAGE_ID_KEY = $"{ACTIVITY_BASE_NAME}.message_id";

    public const string MESSAGE_PAYLOAD_KEY = $"{ACTIVITY_BASE_NAME}.message_payload";

    public const string CACHE_PROVIDER_BASE_NAME = $"{ACTIVITY_BASE_NAME}.hybrid_cache_provider";

    public const string AI_BASE_NAME = $"{ACTIVITY_BASE_NAME}.ai";

    public const string AI_AGENT_NAME_KEY = $"{AI_BASE_NAME}.agent_name";

    public const string AI_REQUEST_MODEL_KEY = $"{AI_BASE_NAME}.request.model";

    public const string AI_REQUEST_TEMPERATURE_KEY = $"{AI_BASE_NAME}.request.temperature";

    public const string AI_REQUEST_MAX_OUTPUT_TOKENS_KEY = $"{AI_BASE_NAME}.request.max_output_tokens";

    public const string AI_RESPONSE_ID_KEY = $"{AI_BASE_NAME}.response.id";

    public const string AI_RESPONSE_MODEL_KEY = $"{AI_BASE_NAME}.response.model";

    public const string AI_RESPONSE_FINISH_REASON_KEY = $"{AI_BASE_NAME}.response.finish_reason";

    public const string AI_USAGE_INPUT_TOKENS_KEY = $"{AI_BASE_NAME}.usage.input_tokens";

    public const string AI_USAGE_OUTPUT_TOKENS_KEY = $"{AI_BASE_NAME}.usage.output_tokens";

    public const string AI_USAGE_TOTAL_TOKENS_KEY = $"{AI_BASE_NAME}.usage.total_tokens";

    public const string CACHE_HIT = $"{CACHE_PROVIDER_BASE_NAME}.cache_hit";

    public const string CACHE_KEY = $"{CACHE_PROVIDER_BASE_NAME}.cache_key";

    public const string CACHE_TAGS = $"{CACHE_PROVIDER_BASE_NAME}.cache_tags";

    public const string API_ROUTE = "api/";
}