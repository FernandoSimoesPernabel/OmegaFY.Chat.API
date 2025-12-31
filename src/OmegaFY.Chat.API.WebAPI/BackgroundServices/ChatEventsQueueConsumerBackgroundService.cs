using OmegaFY.Chat.API.Application.Events;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.MessageBus.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

namespace OmegaFY.Chat.API.WebAPI.BackgroundServices;

public sealed class ChatEventsQueueConsumerBackgroundService : BackgroundService
{
    private static readonly TimeSpan INTERVAL_PERIOD = TimeSpan.FromSeconds(1);

    private readonly IServiceProvider _serviceProvider;

    private readonly IMessageBus _messageBus;

    private readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    private readonly ILogger<ChatEventsQueueConsumerBackgroundService> _logger;

    public ChatEventsQueueConsumerBackgroundService(
        IServiceProvider serviceProvider,
        IMessageBus messageBus,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        ILogger<ChatEventsQueueConsumerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _messageBus = messageBus;
        _openTelemetryRegisterProvider = openTelemetryRegisterProvider;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(INTERVAL_PERIOD);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            MessageEnvelope message = await _messageBus.ReadMessageAsync(stoppingToken);

            if (message is null)
                continue;

            using Activity parentActivity = _openTelemetryRegisterProvider.ContinueParentActivity(OpenTelemetryConstants.ACTIVITY_CHAT_EVENTS_QUEUE_CONSUMER_NAME, message.Headers);
            parentActivity.SetMessage(message);

            Type eventType = message.Payload.GetType();

            using IServiceScope serviceScope = _serviceProvider.CreateScope();

            IEventHandler[] handlers = serviceScope.ServiceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(eventType)).Cast<IEventHandler>().ToArray();

            foreach (IEventHandler handler in handlers)
            {
                try
                {
                    _logger.LogInformation("Handling event {EventType} with handler {HandlerType}", eventType.Name, handler.GetType().Name);

                    using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_EVENT_HANDLER_NAME);
                    activity.SetHandlerName(handler.GetType().Name);

                    await handler.HandleAsync(message.Payload, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling event {EventType} with handler {HandlerType}", eventType.Name, handler.GetType().Name);
                }
            }
        }
    }
}