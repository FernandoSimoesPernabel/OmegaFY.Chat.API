using OmegaFY.Chat.API.Application.Events;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Models;

namespace OmegaFY.Chat.API.WebAPI.BackgroundServices;

public sealed class ChatEventsQueueConsumerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IMessageBus _messageBus;

    private readonly ILogger<ChatEventsQueueConsumerBackgroundService> _logger;

    public ChatEventsQueueConsumerBackgroundService(
        IServiceProvider serviceProvider,
        IMessageBus messageBus,
        ILogger<ChatEventsQueueConsumerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _messageBus = messageBus;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(15));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            MessageEnvelope message = await _messageBus.ReadMessageAync(QueueConstants.CHAT_EVENTS_QUEUE_NAME, stoppingToken);

            if (message is null)
                continue;

            Type eventType = Type.GetType(message.EventType);

            if (eventType is null || message.Payload is null)
            {
                //TODO Log
                await _messageBus.RejectAsync(message, stoppingToken);
            }

            IEventHandler[] handlers = (IEventHandler[])_serviceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(eventType));

            foreach (IEventHandler handler in handlers)
            {
                try
                {
                    await handler.HandleAsync(message.Payload, stoppingToken);

                    await _messageBus.AckAsync(message, stoppingToken);
                }
                catch (Exception ex)
                {
                    //TODO Log
                    await _messageBus.NackAsync(message, stoppingToken);
                }
            }
        }
    }
}