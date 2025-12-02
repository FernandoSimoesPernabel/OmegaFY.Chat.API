using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversations;

public sealed class GetUserConversationsQueryHandler : QueryHandlerBase<GetUserConversationsQueryHandler, GetUserConversationsQuery, GetUserConversationsQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IUserInformation _userInformation;

    public GetUserConversationsQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserConversationsQuery> validator,
        ILogger<GetUserConversationsQueryHandler> logger,
        IChatQueryProvider chatQueryProvider,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<GetUserConversationsQueryResult>> InternalHandleAsync(GetUserConversationsQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetUserConversationsQueryResult>();

        UserConversationModel[] userConversations = await _chatQueryProvider.GetUserConversationsAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        return HandlerResult.Create(new GetUserConversationsQueryResult(userConversations));
    }
}