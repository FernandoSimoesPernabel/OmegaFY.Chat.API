using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Queries.Base;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Chat.GetMessage;

public sealed class GetMessageQueryHandler : QueryHandlerBase<GetMessageQueryHandler, GetMessageQuery, GetMessageQueryResult>
{
    public GetMessageQueryHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<GetMessageQuery> validator) : base(hostEnvironment, openTelemetryRegisterProvider, validator)
    {
    }

    protected override Task<HandlerResult<GetMessageQueryResult>> InternalHandleAsync(GetMessageQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}