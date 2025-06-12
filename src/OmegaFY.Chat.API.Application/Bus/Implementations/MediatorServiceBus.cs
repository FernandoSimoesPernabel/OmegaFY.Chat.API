using MediatR;
using OmegaFY.Chat.API.Application.Shared;
using OmegaFY.Chat.API.Domain.Events;

namespace OmegaFY.Chat.API.Application.Bus.Implementations;

internal sealed class MediatorServiceBus : IServiceBus
{
    private readonly IMediator _mediator;

    public MediatorServiceBus(IMediator mediator) => _mediator = mediator;

    public Task PublishEventAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
        => _mediator.Publish(domainEvent);

    public async Task<TResult> SendMessageAsync<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
        where TRequest : Shared.IRequest
        where TResult : IResult
        => (TResult)await _mediator.Send(request, cancellationToken);
}