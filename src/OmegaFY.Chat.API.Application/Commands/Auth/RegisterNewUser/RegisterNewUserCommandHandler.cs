using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Application.Models;
using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;

namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed class RegisterNewUserCommandHandler : CommandHandlerBase<RegisterNewUserCommandHandler, RegisterNewUserCommand, RegisterNewUserCommandResult>
{
    private readonly IAuthenticationService _authenticationService;

    private readonly HybridCache _hybridCache;

    private readonly IUserRepository _repository;

    public RegisterNewUserCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RegisterNewUserCommand> validator,
        IMessageBus messageBus,
        IAuthenticationService authenticationService,
        HybridCache hybridCache,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus)
    {
        _authenticationService = authenticationService;
        _hybridCache = hybridCache;
        _repository = repository;
    }

    protected async override Task<HandlerResult<RegisterNewUserCommandResult>> InternalHandleAsync(RegisterNewUserCommand command, CancellationToken cancellationToken)
    {
        if (await _repository.CheckIfUserAlreadyExistsAsync(command.Email, cancellationToken))
            throw new ConflictedException();

        User newUser = new User(command.Email, command.DisplayName);

        await _repository.CreateUserAsync(newUser, cancellationToken);

        AuthenticationToken authToken = await _authenticationService.RegisterNewUserAsync(
            new LoginInput(newUser.Id, newUser.Email, command.Password, newUser.DisplayName),
            cancellationToken);

        await _messageBus.RaiseUserRegisteredEventAsync(new UserRegisteredEvent(newUser.Id, newUser.Email, newUser.DisplayName), cancellationToken);

        await _hybridCache.SetAuthenticationTokenCacheAsync(newUser.Id, authToken, cancellationToken);

        return HandlerResult.Create(new RegisterNewUserCommandResult(
            newUser.Id,
            new Token(authToken.Token, authToken.TokenExpirationDate),
            new Token(authToken.RefreshToken, authToken.TokenExpirationDate)));
    }
}