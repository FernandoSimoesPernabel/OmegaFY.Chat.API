using MediatR;

namespace OmegaFY.Chat.API.Application.Commands.Base;

public abstract record class CommandMediatRBase<TResult> : ICommand, IRequest<TResult>
{
}