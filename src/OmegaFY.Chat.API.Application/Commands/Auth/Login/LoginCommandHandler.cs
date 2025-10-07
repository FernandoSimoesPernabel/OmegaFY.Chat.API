using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Auth.Login;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Auth.Login;

public sealed class LoginCommandHandler : CommandHandlerBase<LoginCommandHandler, LoginCommand, LoginCommandResult>
{
    private readonly IAuthenticationService _authenticationService;

    private readonly HybridCache _hybridCache;

    private readonly IUserRepository _repository;

    public LoginCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<LoginCommand> validator,
        IMessageBus messageBus,
        IAuthenticationService authenticationService,
        HybridCache hybridCache,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _authenticationService = authenticationService;
        _hybridCache = hybridCache;
        _repository = repository;
    }

    protected async override Task<HandlerResult<LoginCommandResult>> InternalHandleAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        User user = await _repository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return HandlerResult.CreateUnauthorized<LoginCommandResult>();

        AuthenticationToken authToken =
            await _authenticationService.LoginAsync(new LoginInput(user.Id, user.Email, request.Password, user.DisplayName), cancellationToken);

        if (request.RememberMe)
            await _hybridCache.SetAuthenticationTokenCacheAsync(user.Id, authToken, cancellationToken);

        await _messageBus.SimplePublishAsync(new UserLoggedInEvent(user.Id), cancellationToken);

        return HandlerResult.Create(new LoginCommandResult(
            user.Id,
            user.DisplayName,
            user.Email,
            new Token(authToken.Token, authToken.TokenExpirationDate),
            request.RememberMe ? new Token(authToken.RefreshToken, authToken.RefreshTokenExpirationDate) : null));
    }
}