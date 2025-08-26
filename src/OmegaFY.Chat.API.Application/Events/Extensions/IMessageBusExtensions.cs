using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Constants;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Application.Events.Extensions;

public static class IMessageBusExtensions
{
    public static async Task RaiseUserRegisteredEventAsync(this IMessageBus messageBus, object user, CancellationToken cancellationToken) 
        => await messageBus.SimplePublishAsync(user, cancellationToken);

    public static async Task SimplePublishAsync<TData>(this IMessageBus messageBus, TData payload, CancellationToken cancellationToken)
    {
        MessageEnvelope envelope = new MessageEnvelope()
        {
            DestinationQueue = QueueConstants.DEFAULT_QUEUE_NAME,
            EventType = payload.GetType().FullName,
            Headers = OpenTelemetryPropagatorsHelper.GetInjectedItems(),
            Metadata = [],
            Payload = payload
        };

        await messageBus.PublishAsync(envelope, cancellationToken);
    }
}