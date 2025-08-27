namespace OmegaFY.Chat.API.Infra.Constants;

public static class QueueConstants
{
    public const byte DEFAULT_MAX_RETRY_COUNT = 3;

    public const string CHAT_EVENTS_QUEUE_NAME = "chat-events";

    public const string CHAT_EVENTS_DEAD_LETTER_QUEUE_NAME = $"{CHAT_EVENTS_QUEUE_NAME}-dead-letter";

    public const string DEFAULT_DEAD_LETTER_QUEUE_NAME = "default-dead-letter";
}