namespace OmegaFY.Chat.API.Application.Events;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    public Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}