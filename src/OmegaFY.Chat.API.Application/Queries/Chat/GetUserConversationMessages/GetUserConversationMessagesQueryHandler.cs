using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed class GetUserConversationMessagesQueryHandler : QueryHandlerBase<GetUserConversationMessagesQueryHandler, GetUserConversationMessagesQuery, GetUserConversationMessagesQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IUserInformation _userInformation;

    public GetUserConversationMessagesQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserConversationMessagesQuery> validator,
        IChatQueryProvider chatQueryProvider,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _chatQueryProvider = chatQueryProvider;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<GetUserConversationMessagesQueryResult>> InternalHandleAsync(GetUserConversationMessagesQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetUserConversationMessagesQueryResult>();

        MessageFromMemberModel[] messageFromMembers =
            await _chatQueryProvider.GetMessagesFromMemberAsync(request.ConversationId, _userInformation.CurrentRequestUserId.Value, cancellationToken);

        return HandlerResult.Create(new GetUserConversationMessagesQueryResult(messageFromMembers));
    }
}