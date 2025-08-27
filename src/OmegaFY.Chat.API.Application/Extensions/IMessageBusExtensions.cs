using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class IMessageBusExtensions
{
    public static async Task RaiseUserRegisteredEventAsync(this IMessageBus messageBus, object user, CancellationToken cancellationToken) 
        => await messageBus.SimplePublishAsync(user, cancellationToken);

    public static async Task SimplePublishAsync<TData>(this IMessageBus messageBus, TData payload, CancellationToken cancellationToken) 
        => await messageBus.SimplePublishAsync(QueueConstants.CHAT_EVENTS_QUEUE_NAME, payload, cancellationToken);

    public static async Task SimplePublishAsync<TData>(this IMessageBus messageBus, string destinationQueue, TData payload, CancellationToken cancellationToken)
    {
        MessageEnvelope message = new MessageEnvelope()
        {
            DestinationQueue = destinationQueue,
            EventType = payload.GetType().FullName,
            Headers = OpenTelemetryPropagatorsHelper.GetInjectedItems(),
            Payload = payload
        };

        await messageBus.PublishAsync(message, cancellationToken);
    }
}