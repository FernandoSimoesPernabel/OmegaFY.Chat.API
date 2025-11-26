using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetUserUnreadMessages;

public sealed class GetUserUnreadMessagesQueryHandler : QueryHandlerBase<GetUserUnreadMessagesQueryHandler, GetUserUnreadMessagesQuery, GetUserUnreadMessagesQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    public GetUserUnreadMessagesQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetUserUnreadMessagesQuery> validator,
        IChatQueryProvider chatQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _chatQueryProvider = chatQueryProvider;
    }

    protected override Task<HandlerResult<GetUserUnreadMessagesQueryResult>> InternalHandleAsync(GetUserUnreadMessagesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}