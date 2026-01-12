using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Users;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Users.GetUserById;

public sealed class GetUserByIdQueryHandler : QueryHandlerBase<GetUserByIdQueryHandler, GetUserByIdQuery, GetUserByIdQueryResult>
{
    private readonly IUserQueryProvider _userQueryProvider;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    public GetUserByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserByIdQuery> validator,
        ILogger<GetUserByIdQueryHandler> logger,
        IUserQueryProvider userQueryProvider,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _userQueryProvider = userQueryProvider;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<GetUserByIdQueryResult>> InternalHandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        (_, UserModel user) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.UserByIdKey(request.UserId),
            async (cancellationToken) => await _userQueryProvider.GetUserByIdAsync(request.UserId, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.THIRTY_DAYS,
                LocalCacheExpiration = TimeSpanConstants.THIRTY_DAYS,
                Tags = [CacheTagsGenerator.UsersTag(), CacheTagsGenerator.UsersByIdTag(), CacheTagsGenerator.UsersUserIdTag(request.UserId), CacheTagsGenerator.UserIdTag(request.UserId)]
            },
            cancellationToken);

        if (user is null)
            return HandlerResult.CreateNotFound<GetUserByIdQueryResult>();

        return HandlerResult.Create(new GetUserByIdQueryResult(user));
    }
}