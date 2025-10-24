using OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Common.Helpers;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.Application.Extensions;

public static class IMessageBusExtensions
{
    public static async Task SimplePublishAsync<TData>(this IMessageBus messageBus, TData payload, CancellationToken cancellationToken)
    {
        MessageEnvelope message = new MessageEnvelope()
        {
            Headers = OpenTelemetryPropagatorsHelper.GetInjectedItems(),
            Payload = payload
        };

        await messageBus.PublishAsync(message, cancellationToken);
    }
}