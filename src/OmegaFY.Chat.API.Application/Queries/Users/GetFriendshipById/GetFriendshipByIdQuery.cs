using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetFriendshipById;

public sealed record class GetFriendshipByIdQuery : IQuery
{
    public Guid FriendshipId { get; init; }

    public GetFriendshipByIdQuery() { }

    public GetFriendshipByIdQuery(Guid friendshipId) => FriendshipId = friendshipId;
}

public sealed record class GetFriendshipByIdQueryResult : IQueryResult
{
    public FriendshipModel Friendship { get; init; }

    public GetFriendshipByIdQueryResult() { }

    public GetFriendshipByIdQueryResult(FriendshipModel friendship) => Friendship = friendship;
}

public sealed class GetFriendshipByIdQueryHandler : QueryHandlerBase<GetFriendshipByIdQueryHandler, GetFriendshipByIdQuery, GetFriendshipByIdQueryResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IUserQueryProvider _userQueryProvider;

    public GetFriendshipByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetFriendshipByIdQuery> validator,
        IUserInformation userInformation,
        IUserQueryProvider userQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _userInformation = userInformation;
        _userQueryProvider = userQueryProvider;
    }

    protected async override Task<HandlerResult<GetFriendshipByIdQueryResult>> InternalHandleAsync(GetFriendshipByIdQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<GetFriendshipByIdQueryResult>();

        FriendshipModel friendshipModel = 
            await _userQueryProvider.GetFriendshipByIdAsync(_userInformation.CurrentRequestUserId.Value, request.FriendshipId, cancellationToken);

        if (friendshipModel is null)
            return HandlerResult.CreateNotFound<GetFriendshipByIdQueryResult>();

        return HandlerResult.Create(new GetFriendshipByIdQueryResult(friendshipModel));
    }
}