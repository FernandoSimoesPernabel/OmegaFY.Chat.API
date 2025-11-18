using FluentValidation;
using Microsoft.Extensions.Hosting;
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
        IUserInformation userInformation,
        IChatQueryProvider chatQueryProvider) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _userInformation = userInformation;
        _chatQueryProvider = chatQueryProvider;
    }

    protected override Task<HandlerResult<GetMessageFromMemberQueryResult>> InternalHandleAsync(GetMessageFromMemberQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}