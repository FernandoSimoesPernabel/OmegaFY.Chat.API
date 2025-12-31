using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed class GetMessageFromMemberQueryHandler : QueryHandlerBase<GetMessageFromMemberQueryHandler, GetMessageFromMemberQuery, GetMessageFromMemberQueryResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    public GetMessageFromMemberQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMessageFromMemberQuery> validator,
        ILogger<GetMessageFromMemberQueryHandler> logger,
        IUserInformation userInformation,
        IChatQueryProvider chatQueryProvider,
        IHybridCacheProvider hybridCacheProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _userInformation = userInformation;
        _chatQueryProvider = chatQueryProvider;
        _hybridCacheProvider = hybridCacheProvider;
    }

    protected override async Task<HandlerResult<GetMessageFromMemberQueryResult>> InternalHandleAsync(GetMessageFromMemberQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetMessageFromMemberQueryResult>();

        Guid userId = _userInformation.CurrentRequestUserId.Value;

        (_, MessageFromMemberModel messageFromMember) = await _hybridCacheProvider.GetOrCreateAsync(
            CacheKeyGenerator.MessageFromMemberKey(request.ConversationId, request.MessageId, userId),
            async (cancellationToken) => await _chatQueryProvider.GetMessageFromMemberAsync(request.MessageId, userId, cancellationToken),
            new CacheOptions()
            {
                Expiration = TimeSpanConstants.SEVEN_DAYS,
                LocalCacheExpiration = TimeSpanConstants.SEVEN_DAYS,
                Tags = ["chat", "chat:messages", $"chat:conversation:{request.ConversationId}", $"chat:message:{request.MessageId}", $"chat:user:{userId}"]
            },
            cancellationToken);

        if (messageFromMember is null)
            return HandlerResult.CreateNotFound<GetMessageFromMemberQueryResult>();

        return HandlerResult.Create(new GetMessageFromMemberQueryResult(messageFromMember));
    }
}