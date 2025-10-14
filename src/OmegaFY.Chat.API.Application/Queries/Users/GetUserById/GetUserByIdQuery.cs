using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed record class GetUserByIdQuery : IQuery
{
    public Guid UserId { get; init; }

    public GetUserByIdQuery() { }

    public GetUserByIdQuery(Guid userId) => UserId = userId;
}

public sealed record class GetUserByIdQueryResult : IQueryResult
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string DisplayName { get; init; }

    public FriendshipStatus? FriendshipStatus { get; init; }
}

public sealed class GetUserByIdQueryResultHandler : QueryHandlerBase<GetUserByIdQueryResultHandler, GetUserByIdQuery, GetUserByIdQueryResult>
{
    public GetUserByIdQueryResultHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserByIdQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
    }

    protected override Task<HandlerResult<GetUserByIdQueryResult>> InternalHandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}