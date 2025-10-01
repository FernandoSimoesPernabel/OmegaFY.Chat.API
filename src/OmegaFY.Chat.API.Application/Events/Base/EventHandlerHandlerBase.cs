namespace OmegaFY.Chat.API.Application.Events.Base;

internal abstract class EventHandlerHandlerBase<TEvent> : IEventHandler<TEvent>
{
    public async Task HandleAsync(object @event, CancellationToken cancellationToken) => await HandleAsync((TEvent)@event, cancellationToken);
    
    protected abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}