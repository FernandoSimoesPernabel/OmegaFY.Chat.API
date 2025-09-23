using OmegaFY.Chat.API.Application.Events;
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
            MessageEnvelope message = await _messageBus.ReadMessageAsync(stoppingToken);

            if (message is null)
                continue;

            Type eventType = message.Payload.GetType();

            IEventHandler[] handlers = (IEventHandler[])_serviceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(eventType));

            foreach (IEventHandler handler in handlers)
            {
                try
                {
                    await handler.HandleAsync(message.Payload, stoppingToken);
                }
                catch (Exception ex)
                {
                    //TODO Log
                }
            }
        }
    }
}