using FluentValidation;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;
using OmegaFY.Chat.API.Application.Extensions;
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

namespace OmegaFY.Chat.API.Application.Commands.Auth.RegisterNewUser;

public sealed class RegisterNewUserCommandHandler : CommandHandlerBase<RegisterNewUserCommandHandler, RegisterNewUserCommand, RegisterNewUserCommandResult>
{
    private readonly IAuthenticationService _authenticationService;

    private readonly IHybridCacheProvider _hybridCacheProvider;

    private readonly IUserRepository _repository;

    public RegisterNewUserCommandHandler(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<RegisterNewUserCommand> validator,
        IMessageBus messageBus,
        ILogger<RegisterNewUserCommandHandler> logger,
        IAuthenticationService authenticationService,
        IHybridCacheProvider hybridCacheProvider,
        IUserRepository repository) : base(hostEnvironment, openTelemetryRegisterProvider, validator, messageBus, logger)
    {
        _authenticationService = authenticationService;
        _hybridCacheProvider = hybridCacheProvider;
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

        await _hybridCacheProvider.SetAuthenticationTokenCacheAsync(newUser.Id, authToken, cancellationToken);
        
        await _messageBus.SimplePublishAsync(new UserRegisteredEvent(newUser.Id, newUser.Email, newUser.DisplayName), cancellationToken);

        return HandlerResult.Create(new RegisterNewUserCommandResult(
            newUser.Id,
            new Token(authToken.Token, authToken.TokenExpirationDate),
            new Token(authToken.RefreshToken, authToken.TokenExpirationDate)));
    }
}