using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Auth.Logoff;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Auth.Logoff;

public sealed class LogoffCommandHandler : CommandHandlerBase<LogoffCommandHandler, LogoffCommand, LogoffCommandResult>
{
    private readonly IUserInformation _userInformation;

    public LogoffCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<LogoffCommand> validator,
        IMessageBus messageBus,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _userInformation = userInformation;
    }

    protected override async Task<HandlerResult<LogoffCommandResult>> InternalHandleAsync(LogoffCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthorized<LogoffCommandResult>();

        Guid userId = _userInformation.CurrentRequestUserId.Value;

        await _messageBus.SimplePublishAsync(new UserLoggedOffEvent(userId, request.RefreshToken), cancellationToken);

        return HandlerResult.Create(new LogoffCommandResult());
    }
}