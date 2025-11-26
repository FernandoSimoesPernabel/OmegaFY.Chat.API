using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserConversationMessages;

public sealed class GetUserConversationMessagesQueryHandler : QueryHandlerBase<GetUserConversationMessagesQueryHandler, GetUserConversationMessagesQuery, GetUserConversationMessagesQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    public GetUserConversationMessagesQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserConversationMessagesQuery> validator,
        IChatQueryProvider chatQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _chatQueryProvider = chatQueryProvider;
    }

    protected override Task<HandlerResult<GetUserConversationMessagesQueryResult>> InternalHandleAsync(GetUserConversationMessagesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}