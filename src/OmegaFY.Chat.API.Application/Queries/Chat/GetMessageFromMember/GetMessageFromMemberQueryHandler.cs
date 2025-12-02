using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessageFromMember;

public sealed class GetMessageFromMemberQueryHandler : QueryHandlerBase<GetMessageFromMemberQueryHandler, GetMessageFromMemberQuery, GetMessageFromMemberQueryResult>
{
    private readonly IUserInformation _userInformation;

    private readonly IChatQueryProvider _chatQueryProvider;

    public GetMessageFromMemberQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMessageFromMemberQuery> validator,
        ILogger<GetMessageFromMemberQueryHandler> logger,
        IUserInformation userInformation,
        IChatQueryProvider chatQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _userInformation = userInformation;
        _chatQueryProvider = chatQueryProvider;
    }

    protected override async Task<HandlerResult<GetMessageFromMemberQueryResult>> InternalHandleAsync(GetMessageFromMemberQuery request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<GetMessageFromMemberQueryResult>();

        MessageFromMemberModel messageFromMember = await _chatQueryProvider.GetMessageFromMemberAsync(request.MessageId, _userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (messageFromMember is null)
            return HandlerResult.CreateNotFound<GetMessageFromMemberQueryResult>();

        return HandlerResult.Create(new GetMessageFromMemberQueryResult(messageFromMember));
    }
}