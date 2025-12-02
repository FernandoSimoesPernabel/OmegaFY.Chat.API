using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMemberFromConversation;

public sealed class GetMemberFromConversationQueryHandler : QueryHandlerBase<GetMemberFromConversationQueryHandler, GetMemberFromConversationQuery, GetMemberFromConversationQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    public GetMemberFromConversationQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMemberFromConversationQuery> validator,
        ILogger<GetMemberFromConversationQueryHandler> logger,
        IChatQueryProvider chatQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
    }

    protected override async Task<HandlerResult<GetMemberFromConversationQueryResult>> InternalHandleAsync(GetMemberFromConversationQuery request, CancellationToken cancellationToken)
    {
        MemberModel member = await _chatQueryProvider.GetMemberByIdAsync(request.MemberId, cancellationToken);

        if (member is null)
            return HandlerResult.CreateNotFound<GetMemberFromConversationQueryResult>();

        return HandlerResult.Create(new GetMemberFromConversationQueryResult(member));
    }
}