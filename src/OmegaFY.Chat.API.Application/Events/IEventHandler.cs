namespace OmegaFY.Chat.API.Application.Events;

public interface IEventHandler
{
    public Task HandleAsync(object @event, CancellationToken cancellationToken);
}