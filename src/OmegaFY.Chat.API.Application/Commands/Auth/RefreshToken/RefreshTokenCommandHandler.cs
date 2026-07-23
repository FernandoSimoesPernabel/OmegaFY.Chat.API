using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Auth.RefreshToken;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Helpers;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler : CommandHandlerBase<RefreshTokenCommandHandler, RefreshTokenCommand, RefreshTokenCommandResult>
{
    private readonly IHybridCacheProvider _hybridCacheProvider;

    private readonly IUserRepository _repository;

    private readonly IAuthenticationService _authenticationService;

    public RefreshTokenCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RefreshTokenCommand> validator,
        IMessageBus messageBus,
        ILogger<RefreshTokenCommandHandler> logger,
        IHybridCacheProvider hybridCacheProvider,
        IUserRepository repository,
        IAuthenticationService authenticationService) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus, logger)
    {
        _hybridCacheProvider = hybridCacheProvider;
        _repository = repository;
        _authenticationService = authenticationService;
    }

    protected async override Task<HandlerResult<RefreshTokenCommandResult>> InternalHandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        User user = await _repository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return HandlerResult.CreateUnauthorized<RefreshTokenCommandResult>();

        (_, AuthenticationToken? currentToken) = await _hybridCacheProvider.GetOrDefaultAsync<AuthenticationToken?>(
            CacheKeyGenerator.RefreshTokenKey(user.Id, request.RefreshToken),
            cancellationToken);

        if (!currentToken.HasValue || request.CurrentToken != currentToken.Value.Token)
            return HandlerResult.CreateForbidden<RefreshTokenCommandResult>();

        AuthenticationToken newAuthToken = 
            await _authenticationService.RefreshTokenAsync(new RefreshTokenInput(user.Id, user.Email, user.DisplayName), cancellationToken);

        await _hybridCacheProvider.SetAuthenticationTokenAsync(user.Id, newAuthToken, cancellationToken);

        await _messageBus.SimplePublishAsync(new UserTokenRefreshedEvent(user.Id, request.RefreshToken, newAuthToken.RefreshToken), cancellationToken);

        return HandlerResult.Create(new RefreshTokenCommandResult(
            new Token(user.Id, newAuthToken.Token, newAuthToken.TokenExpirationDate),
            new Token(user.Id, newAuthToken.RefreshToken, newAuthToken.RefreshTokenExpirationDate)));
    }
}