The goal of these instructions is to make an AI coding agent immediately productive in the OmegaFY.Chat.API repository.

Keep answers concise and code-focused. When changing code, prefer small, well-tested edits that follow existing patterns.

Quick architecture summary
- Multi-project .NET 7+ solution (C#): main projects under `src/` are:
  - `OmegaFY.Chat.API.WebAPI` — the ASP.NET Web API host (startup, DI registration, controllers, OpenAPI)
  - `OmegaFY.Chat.API.Application` — application layer: Commands, Queries, Events, handlers, validators
  - `OmegaFY.Chat.API.Domain` — domain entities and repository interfaces
  - `OmegaFY.Chat.API.Data.EF` — Entity Framework Core DB context, repositories, EF-specific user manager
  - `OmegaFY.Chat.API.Infra` — infra services (Authentication, MessageBus, OpenTelemetry, Cache)
  - `OmegaFY.Chat.API.Common` — shared helpers, exceptions, constants

Key patterns and where to find them
- Dependency registration is automatic: `OmegaFY.Chat.API.WebAPI.Extensions.DependencyInjectionExtensions.AddDependencyInjectionRegister` scans `WebAPI` assembly for implementations of `IDependencyInjectionRegister` (see `src/.../WebAPI/DependencyInjection/Registrations/*`). Use this pattern when adding new cross-cutting registrations.
- Handler pipeline: application handlers inherit `Application.Shared.HandlerBase<THandler,TRequest,TResult>` which performs:
  - validation using FluentValidation (validator injected as `IValidator<TRequest>`)
  - OpenTelemetry activity creation via `IOpenTelemetryRegisterProvider`
  - exception to `HandlerResult` mapping (see `HandlerBase.cs`)
  Example handlers/registrations: `src/.../Application/Extensions/DependencyInjectionExtensions.cs` and `HandlersRegistration.cs` in `WebAPI`.
- Commands / Queries / Events: follow the Commands/Queries/Events folders inside `Application`. Register handlers in `Application.Extensions.DependencyInjectionExtensions`.
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
- Adding a new API endpoint that triggers a command:
  1. Create Command DTO in `Application/Commands/...` and a matching `IValidator<TCommand>` using FluentValidation.
  2. Implement `...CommandHandler : HandlerBase<...>` in `Application/Commands/...` and implement `InternalHandleAsync`.
  3. Register handler in `Application.Extensions.DependencyInjectionExtensions.AddCommandHandlers()`.
  4. Add controller action in `WebAPI/Controllers/*` that depends on mediator-like dispatching (check existing controllers for pattern).

Edge cases for the agent to watch for
- Don't bypass `HandlerBase`'s validation/exception mapping — it's how errors are normalized.
- When changing DI registrations, keep them idempotent and registered with appropriate lifetime (scoped/singleton) consistent with surrounding registrations.
- EF contexts are added with pooling (`AddDbContextPool`) — avoid storing DbContext as a singleton.

Where to look for examples and reference files
- `src/OmegaFY.Chat.API.WebAPI/Program.cs` (application start)
- `src/OmegaFY.Chat.API.WebAPI/DependencyInjection/Registrations/*` (where cross-cutting services are registered)
- `src/OmegaFY.Chat.API.Application/Shared/HandlerBase.cs` (handler pipeline)
- `src/OmegaFY.Chat.API.Infra/Extensions/DependencyInjectionExtensions.cs` (auth, message bus, OpenTelemetry)
- `src/OmegaFY.Chat.API.Data.EF/Extensions/DependencyInjectionExtensions.cs` (EF stores, repositories)
- Tests: `test/OmegaFY.Chat.API.Tests.Unit/Infra/MessageBus/*` for message bus behavior

If you change behavior or add features, run the unit tests scoped to affected projects before committing.

If anything in this file is unclear or you'd like more examples (controllers, a sample handler + validator, or a short checklist for PRs), tell me what to expand and I'll update the file.
