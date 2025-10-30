The goal of these instructions is to make an AI coding agent immediately productive in the OmegaFY.Chat.API repository.

This is a Chat API built with ASP.NET Core, using CQRS-style command/query separation with message bus for decoupled event processing. Core domains: users, authentication, and real-time chat features. The architecture emphasizes maintainability through clear separation of concerns and standardized handler/validation patterns.

Keep answers concise and code-focused. When changing code, prefer small, well-tested edits that follow existing patterns.

Quick architecture summary
- Multi-project .NET 7+ solution (C#): main projects under `src/` are:
  - `OmegaFY.Chat.API.WebAPI` — the ASP.NET Web API host (startup, DI registration, controllers, OpenAPI)
  - `OmegaFY.Chat.API.Application` — application layer: Commands, Queries, Events, handlers, validators
  - `OmegaFY.Chat.API.Domain` — domain entities and repository interfaces
  - `OmegaFY.Chat.API.Data.EF` — Entity Framework Core DB context, repositories, EF-specific user manager
  - `OmegaFY.Chat.API.Data.Dapper` — Dapper query providers for optimized read operations
  - `OmegaFY.Chat.API.Infra` — infra services (Authentication, MessageBus, OpenTelemetry, Cache)
  - `OmegaFY.Chat.API.Common` — shared helpers, exceptions, constants

Key patterns and where to find them
- Dependency registration is automatic: `OmegaFY.Chat.API.WebAPI.Extensions.DependencyInjectionExtensions.AddDependencyInjectionRegister` scans `WebAPI` assembly for implementations of `IDependencyInjectionRegister` (see `src/.../WebAPI/DependencyInjection/Registrations/*`). Use this pattern when adding new cross-cutting registrations.
- Handler pipeline: application handlers inherit `Application.Shared.HandlerBase<THandler,TRequest,TResult>` which performs:
  - validation using FluentValidation (validator injected as `IValidator<TRequest>`)
  - OpenTelemetry activity creation via `IOpenTelemetryRegisterProvider`
  - exception to `HandlerResult` mapping (see `HandlerBase.cs`)
  Example handlers/registrations: `src/.../Application/Extensions/DependencyInjectionExtensions.cs` and `HandlersRegistration.cs` in `WebAPI`.
- Commands / Queries / Events flow:
  - Commands: state-changing operations, validated by FluentValidation, return `HandlerResult<TResult>`
  - Queries: read-only data access through EF or Dapper query providers
  - Events: asynchronous side effects triggered via message bus (e.g., `SendWelcomeEmailEvent` after user registration)
  Register handlers in `Application.Extensions.DependencyInjectionExtensions`.
- Message bus: in-memory message buses live under `Infra.MessageBus`. Tests for message buses are in `test/.../Infra/MessageBus`. Use `AddConcurrentBagInMemoryMessageBus()` or `AddChannelInMemoryMessageBus()` from `Infra.Extensions.DependencyInjectionExtensions`.
- OpenTelemetry: configured in `Infra.Extensions.DependencyInjectionExtensions.AddOpenTelemetry(...)` and registered by `WebAPI.DependencyInjection.Registrations.OpenTelemetryRegistration`. Activities are started by the application `HandlerBase` and `OpenTelemetryActivitySourceProvider` provides the ActivitySource.
- Authentication: Identity + JWT configured in `Infra.Extensions.DependencyInjectionExtensions.AddIdentityUserConfiguration`. Identity EF stores are wired in `Data.EF.Extensions.DependencyInjectionExtensions`. Jwt settings are read from configuration (`appsettings.json`) and `JwtSettings` model.

Developer workflows (commands)
- Restore, build and run the WebAPI (from repository root):
  - dotnet restore OmegaFY.Chat.API.sln
  - dotnet build OmegaFY.Chat.API.sln -c Debug
  - dotnet run --project src/OmegaFY.Chat.API.WebAPI --no-launch-profile
- Run unit tests (run all unit tests project):
  - dotnet test test/OmegaFY.Chat.API.Tests.Unit/OmegaFY.Chat.API.Tests.Unit.csproj
- Run benchmark tests (if needed):
  - dotnet run --project test/OmegaFY.Chat.API.Tests.Benchmark

Project-specific conventions
- Prefer DI registration via the `IDependencyInjectionRegister` pattern in `WebAPI/DependencyInjection/Registrations`. New registrations should be placed there rather than scattering calls across Program.cs.
- Handlers must validate requests using FluentValidation. Validators are registered by convention (inject `IValidator<TRequest>`). See `HandlerBase` for expected behavior on validation/exception mapping.
- Use `HandlerResult<TResult>` and `HandlerResult` patterns for handler outputs (found in `Application.Shared`). Do not return raw domain exceptions — let `HandlerBase` map exceptions.
- Telemetry: always create Activities on long-running operations by using provided `IOpenTelemetryRegisterProvider` or rely on `HandlerBase` which does this automatically for handlers.

Integration points and external dependencies
- Database: EF Core with SQL Server (connection string key `AzureSql` in configuration). Context: `Data.EF.Context.ApplicationContext`.
- Identity: ASP.NET Core Identity with EF stores + JWT. Relevant files:
  - `src/.../Infra/Extensions/DependencyInjectionExtensions.cs` (AddIdentityUserConfiguration)
  - `src/.../Data.EF/Extensions/DependencyInjectionExtensions.cs` (AddEntityFrameworkStores)
- OpenTelemetry / Honeycomb: configured in `Infra` and wired by registration in `WebAPI`. Package refs in `src/OmegaFY.Chat.API.Infra/OmegaFY.Chat.API.Infra.csproj`.

Examples the agent can follow
- Adding a new command (state-changing operation):
  1. Create folder structure in `Application/Commands/YourDomain/YourCommand/`
  2. Create command DTO + validator:
     - Command must implement `ICommand`
     - Validator must inherit `AbstractValidator<YourCommand>` 
  3. Create handler inheriting `HandlerBase<THandler, TCommand, TResult>`
  4. Create any related events that should be triggered after command:
     - Event class implementing `IEvent`
     - Event handler implementing `IEventHandler<TEvent>`
     - Use `IMessageBus.PublishAsync(event)` in command handler
     - Register event handler in `AddEventHandlers()`
  5. Register command handler in `Application.Extensions.DependencyInjectionExtensions.AddCommandHandlers()`
  6. Add controller action that injects and calls the handler

- Adding a new query (read-only operation):
  1. Create folder structure in `Application/Queries/YourDomain/YourQuery/`
  2. Create query DTO implementing `IQuery` + validator
  3. Create query provider interface in `Application/Queries/QueryProviders/YourDomain/` 
  4. Implement query provider in `Data.Dapper/QueryProviders/YourDomain/`
  5. Register query provider in `Data.Dapper.Extensions.DependencyInjectionExtensions`
  6. Create handler inheriting `QueryHandlerBase<THandler, TQuery, TResult>`
  7. Register handler in `Application.Extensions.DependencyInjectionExtensions.AddQueryHandlers()`
  8. Add controller action

Here's a complete command example (RegisterNewUser):
  1. Command DTO + Validator:
     ```csharp
     public sealed record RegisterNewUserCommand(string Email, string Password) : ICommand;
     
     public sealed class RegisterNewUserCommandValidator : AbstractValidator<RegisterNewUserCommand>
     {
         public RegisterNewUserCommandValidator()
         {
             RuleFor(x => x.Email).NotEmpty().EmailAddress();
             RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
         }
     }
     ```
  2. Handler with validation + telemetry:
     ```csharp
     // 1. Event definition
     public sealed record SendWelcomeEmailEvent(Guid UserId) : IEvent;

     // 2. Event handler
     public sealed class SendWelcomeEmailEventHandler : IEventHandler<SendWelcomeEmailEvent>
     {
         public Task HandleAsync(object @event, CancellationToken cancellationToken)
         {
             if (@event is SendWelcomeEmailEvent welcomeEvent)
             {
                 // Handle welcome email sending
                 return Task.CompletedTask;
             }
             throw new ArgumentException($"Event must be of type {nameof(SendWelcomeEmailEvent)}");
         }
     }

     // 3. Command handler that publishes event
     public sealed class RegisterNewUserCommandHandler 
         : HandlerBase<RegisterNewUserCommandHandler, RegisterNewUserCommand, RegisterNewUserCommandResult>
     {
         private readonly IUserManager _userManager;
         private readonly IMessageBus _messageBus;
         
         public RegisterNewUserCommandHandler(
             IHostEnvironment env,
             IOpenTelemetryRegisterProvider telemetry,
             IValidator<RegisterNewUserCommand> validator,
             IUserManager userManager,
             IMessageBus messageBus) : base(env, telemetry, validator)
         {
             _userManager = userManager;
             _messageBus = messageBus;
         }
         
         protected override async Task<HandlerResult<RegisterNewUserCommandResult>> InternalHandleAsync(
             RegisterNewUserCommand command, 
             CancellationToken cancellationToken)
         {
             var user = await _userManager.CreateUserAsync(command.Email, command.Password);
             // Publish event via message bus for async handling
             await _messageBus.PublishAsync(new SendWelcomeEmailEvent(user.Id), cancellationToken);
             return new HandlerResult<RegisterNewUserCommandResult>(new(user.Id));
         }
     }

     // 4. Register event handler in DI
     public static IServiceCollection AddEventHandlers(this IServiceCollection services)
     {
         services.AddScoped<SendWelcomeEmailEventHandler>();
         return services;
     }
     ```
  3. Register in DI:
     ```csharp
     public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
     {
         services.AddScoped<RegisterNewUserCommandHandler>();
         return services;
     }
     ```
  4. Controller action:
     ```csharp
     [HttpPost("register")]
     public async Task<IActionResult> Register(
         [FromBody] RegisterNewUserCommand command,
         [FromServices] RegisterNewUserCommandHandler handler,
         CancellationToken cancellationToken)
     {
         HandlerResult<RegisterNewUserCommandResult> result = 
             await handler.HandleAsync(command, cancellationToken);
         return result.ToActionResult();
     }
     ```

Edge cases for the agent to watch for
- Don't bypass `HandlerBase`'s validation/exception mapping — it's how errors are normalized.
- When changing DI registrations, keep them idempotent and registered with appropriate lifetime:
  - Command/Query handlers: `scoped`
  - Query providers (Dapper): `scoped` 
  - Message bus: `singleton`
  - EF contexts: use `AddDbContextPool`, never store as singleton
- Queries should use Dapper query providers for better read performance
- Commands should use EF Core repositories for transactional writes

Where to look for examples and reference files
- `src/OmegaFY.Chat.API.WebAPI/Program.cs` (application start)
- `src/OmegaFY.Chat.API.WebAPI/DependencyInjection/Registrations/*` (where cross-cutting services are registered)
- `src/OmegaFY.Chat.API.Application/Shared/HandlerBase.cs` (handler pipeline)
- `src/OmegaFY.Chat.API.Infra/Extensions/DependencyInjectionExtensions.cs` (auth, message bus, OpenTelemetry)
- `src/OmegaFY.Chat.API.Data.EF/Extensions/DependencyInjectionExtensions.cs` (EF stores, repositories)
- Tests: `test/OmegaFY.Chat.API.Tests.Unit/Infra/MessageBus/*` for message bus behavior

If you change behavior or add features, run the unit tests scoped to affected projects before committing.

If anything in this file is unclear or you'd like more examples (controllers, a sample handler + validator, or a short checklist for PRs), tell me what to expand and I'll update the file.
