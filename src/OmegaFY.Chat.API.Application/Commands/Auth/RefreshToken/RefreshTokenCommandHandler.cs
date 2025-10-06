using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Common.Exceptions;
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
        HybridCache hybridCache,
        IUserRepository repository,
        IAuthenticationService authenticationService,
        IUserInformation userInformation) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _hybridCache = hybridCache;
        _repository = repository;
        _authenticationService = authenticationService;
        _userInformation = userInformation;
    }

    protected async override Task<HandlerResult<RefreshTokenCommandResult>> InternalHandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_userInformation.IsAuthenticated)
            HandlerResult.CreateUnauthorized<RefreshTokenCommandResult>();

        User user = await _repository.GetByIdAsync(_userInformation.CurrentRequestUserId.Value, cancellationToken);

        if (user is null)
            return HandlerResult.CreateNotFound<RefreshTokenCommandResult>();

        AuthenticationToken? currentToken = await _hybridCache.GetOrDefaultAsync<AuthenticationToken?>(
            CacheKeyGenerator.RefreshTokenKey(_userInformation.CurrentRequestUserId.Value, request.RefreshToken), 
            cancellationToken);

        if (!currentToken.HasValue)
            throw new NotFoundException();

        if (request.CurrentToken != currentToken.Value.Token)
            throw new DomainInvalidOperationException("O token informado não corresponde ao token armazenado.");

        AuthenticationToken newAuthToken = await _authenticationService.RefreshTokenAsync(
            currentToken.Value,
            new RefreshTokenInput(user.Id, user.Email, user.DisplayName),
            cancellationToken);

        await _hybridCache.RemoveAuthenticationTokenCacheAsync(user.Id, request.RefreshToken, cancellationToken);

        await _hybridCache.SetAuthenticationTokenCacheAsync(user.Id, newAuthToken, cancellationToken);

        return HandlerResult.Create(new RefreshTokenCommandResult(
            new Token(newAuthToken.Token, newAuthToken.TokenExpirationDate),
            new Token(newAuthToken.RefreshToken, newAuthToken.RefreshTokenExpirationDate)));
    }
}