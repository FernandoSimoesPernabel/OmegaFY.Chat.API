using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Auth.Logoff;

public sealed class LogoffCommandHandler : CommandHandlerBase<LogoffCommandHandler, LogoffCommand, LogoffCommandResult>
{
    private readonly HybridCache _hybridCache;

    private readonly IUserInformation _userInformation;

    public LogoffCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<LogoffCommand> validator,
        IMessageBus messageBus,
        HybridCache hybridCache,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _hybridCache = hybridCache;
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<LogoffCommandResult>> InternalHandleAsync(LogoffCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<LogoffCommandResult>();

        await _hybridCache.RemoveAsync(CacheKeyGenerator.RefreshTokenKey(_userInformation.CurrentRequestUserId.Value, request.RefreshToken), cancellationToken);

        return HandlerResult.Create(new LogoffCommandResult());
    }
}