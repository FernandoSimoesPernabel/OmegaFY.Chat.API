using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Queries.Base;

public abstract class QueryHandlerBase<TQueryHandler, TQuery, TQueryResult> : HandlerBase<TQueryHandler, TQuery, TQueryResult>, IQueryHandler<TQuery, TQueryResult>
    where TQuery : IQuery
    where TQueryResult : IQueryResult
{
    protected QueryHandlerBase(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<TQuery> validator,
        ILogger<TQueryHandler> logger) : base(hostEnvironment, openTelemetryRegisterProvider, validator, logger) { }
}