using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Extensions;
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

    private readonly IDistributedCache _distributedCache;

    private readonly IUserRepository _repository;

    public RegisterNewUserCommandHandler(
        IMessageBus messageBus,
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IAuthenticationService authenticationService,
        IDistributedCache distributedCache,
        IUserRepository repository,
        IValidator<RegisterNewUserCommand> validator) : base(messageBus, hostEnvironment, openTelemetryRegisterProvider, validator)
    {
        _authenticationService = authenticationService;
        _distributedCache = distributedCache;
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

        await _messageBus.RaiseUserRegisteredEventAsync(newUser, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        await _distributedCache.SetAuthenticationTokenCacheAsync(newUser.Id, authToken, cancellationToken);

        return HandlerResult.Create(new RegisterNewUserCommandResult(
            newUser.Id,
            authToken.Token,
            authToken.TokenExpirationDate,
            authToken.RefreshToken,
            authToken.RefreshTokenExpirationDate));
    }
}