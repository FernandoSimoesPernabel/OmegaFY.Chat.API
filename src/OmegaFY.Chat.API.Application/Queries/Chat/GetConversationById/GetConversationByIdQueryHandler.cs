using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Application.Queries.QueryProviders.Chat;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetConversationById;

public sealed class GetConversationByIdQueryHandler : QueryHandlerBase<GetConversationByIdQueryHandler, GetConversationByIdQuery, GetConversationByIdQueryResult>
{
    private readonly IChatQueryProvider _chatQueryProvider;

    public GetConversationByIdQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetConversationByIdQuery> validator,
        ILogger<GetConversationByIdQueryHandler> logger,
        IChatQueryProvider chatQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger)
    {
        _chatQueryProvider = chatQueryProvider;
    }

    protected override async Task<HandlerResult<GetConversationByIdQueryResult>> InternalHandleAsync(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        ConversationAndMembersModel conversation = await _chatQueryProvider.GetConversationByIdAsync(request.ConversationId, cancellationToken);

        if (conversation is null)
            return HandlerResult.CreateNotFound<GetConversationByIdQueryResult>();

        return HandlerResult.Create(new GetConversationByIdQueryResult(conversation));
    }
}