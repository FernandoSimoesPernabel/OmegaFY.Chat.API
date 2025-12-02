using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler : CommandHandlerBase<RefreshTokenCommandHandler, RefreshTokenCommand, RefreshTokenCommandResult>
{
    private readonly HybridCache _hybridCache;

    private readonly IUserRepository _repository;

    private readonly IAuthenticationService _authenticationService;

    private readonly IUserInformation _userInformation;

    public RefreshTokenCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RefreshTokenCommand> validator,
        IMessageBus messageBus,
        ILogger<RefreshTokenCommandHandler> logger,
        HybridCache hybridCache,
        IUserRepository repository,
        IAuthenticationService authenticationService,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus, logger)
    {
        _hybridCache = hybridCache;
        _repository = repository;
        _authenticationService = authenticationService;
        _userInformation = userInformation;
    }

    protected async override Task<HandlerResult<RefreshTokenCommandResult>> InternalHandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            return HandlerResult.CreateUnauthenticated<RefreshTokenCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is null)
            return HandlerResult.CreateUnauthorized<RefreshTokenCommandResult>();

        AuthenticationToken? currentToken = await _hybridCache.GetOrDefaultAsync<AuthenticationToken?>(
            CacheKeyGenerator.RefreshTokenKey(_userInformation.CurrentRequestUserId.Value, request.RefreshToken), 
            cancellationToken);

        if (!currentToken.HasValue || request.CurrentToken != currentToken.Value.Token)
            return HandlerResult.CreateUnauthorized<RefreshTokenCommandResult>();

        AuthenticationToken newAuthToken = await _authenticationService.RefreshTokenAsync(
            currentToken.Value,
            new RefreshTokenInput(user.Id, user.Email, user.DisplayName),
            cancellationToken);

        await _hybridCache.SetAuthenticationTokenCacheAsync(user.Id, newAuthToken, cancellationToken);

        await _messageBus.SimplePublishAsync(new UserTokenRefreshedEvent(user.Id, request.RefreshToken, newAuthToken.RefreshToken), cancellationToken);

        return HandlerResult.Create(new RefreshTokenCommandResult(
            new Token(newAuthToken.Token, newAuthToken.TokenExpirationDate),
            new Token(newAuthToken.RefreshToken, newAuthToken.RefreshTokenExpirationDate)));
    }
}