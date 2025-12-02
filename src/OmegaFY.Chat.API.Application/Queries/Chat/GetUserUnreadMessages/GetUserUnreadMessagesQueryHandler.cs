using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Common.Models;
using OmegaFY.Chat.API.Domain.Enums;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;

public sealed class GetUserUnreadMessagesQueryHandler : QueryHandlerBase<GetUserUnreadMessagesQueryHandler, GetUserUnreadMessagesQuery, GetUserUnreadMessagesQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    private readonly IUserInformation _userInformation;

    public GetUserUnreadMessagesQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserUnreadMessagesQuery> validator,
        ILogger<GetUserUnreadMessagesQueryHandler> logger,
        IChatQueryProvider chatQueryProvider,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<GetUserUnreadMessagesQueryResult>> InternalHandleAsync(GetUserUnreadMessagesQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetUserUnreadMessagesQueryResult>();

        (MessageModel[] unreadMessagesFromUser, PaginationResultInfo paginationInfo) = 
            await _chatQueryProvider.GetMessagesFromUserAsync(_userInformation.CurrentRequestUserId.Value, MemberMessageStatus.Unread, request.Pagination, cancellationToken);

        return HandlerResult.Create(new GetUserUnreadMessagesQueryResult(unreadMessagesFromUser, paginationInfo));
    }
}