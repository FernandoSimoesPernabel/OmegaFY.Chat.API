using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Domain.Events;

namespace OmegaFY.Chat.API.Application.Bus;

public interface IServiceBus
{
    public Task<TResult> SendMessageAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest where TResult : IResult;

    public Task PublishEventAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
}