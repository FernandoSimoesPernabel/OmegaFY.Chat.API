using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class MessageEnvelopeExtensions
{
    public static string GetDeadLetterQueue(this MessageEnvelope message)
    {
        return message.DestinationQueue switch
        {
            QueueConstants.CHAT_EVENTS_QUEUE_NAME => QueueConstants.CHAT_EVENTS_DEAD_LETTER_QUEUE_NAME,
            _ => QueueConstants.DEFAULT_DEAD_LETTER_QUEUE_NAME,
        };
    }
}