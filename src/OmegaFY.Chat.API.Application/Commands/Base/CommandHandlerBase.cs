using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Shared.Extensions;
using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.MessageBus;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Application.Commands.Base;

//TODO provavelmente vai fazer sentido ter mais um base, porque query vai fazer boa parte disso
public abstract class CommandHandlerBase<TCommandHandler, TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    protected readonly IMessageBus _messageBus;

    protected readonly IHostEnvironment _hostEnvironment;

    protected readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    protected readonly IValidator<TCommand> _validator;

    protected CommandHandlerBase(
        IMessageBus messageBus,
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<TCommand> validator)
    {
        _messageBus = messageBus;
        _hostEnvironment = hostEnvironment;
        _openTelemetryRegisterProvider = openTelemetryRegisterProvider;
        _validator = validator;
    }

    public async Task<HandlerResult<TCommandResult>> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_APPLICATION_HANDLER_NAME);
        activity.SetHandlerName(GetType().Name);

        try
        {
            activity.SetRequest(command);

            ValidationResult validationResult = _validator.Validate(command);

            /*
           return !validationResult.IsValid ? ErrosFromValidationFailure(validationResult.Errors) : await next();


                private static TResult ErrosFromValidationFailure(IEnumerable<ValidationFailure> failures)
   {
       TResult result = CreateInstanceOfTResult();

       foreach (ValidationFailure failure in failures)
           result.AddError(ApplicationErrorCodes.DOMAIN_ARGUMENT_INVALID, failure.ErrorMessage);

       return result;
   }

   private static TResult ErrorsFromException(string errorCode, string errorMessage)
   {
       TResult result = CreateInstanceOfTResult();
       result.AddError(errorCode, errorMessage);

       return result;
   }

   private static TResult CreateInstanceOfTResult() => (TResult)Activator.CreateInstance(typeof(TResult));
            */

            HandlerResult<TCommandResult> result = await InternalHandleAsync(command, cancellationToken);

            activity.SetResult(result);

            return result;
        }
        catch (ErrorCodeException ex)
        {
            activity.AddException(ex);
            return new HandlerResult<TCommandResult>(ex.ErrorCode, ex.Message);
        }
        catch (Exception ex)
        {
            activity.AddException(ex);
            return new HandlerResult<TCommandResult>(ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR, ex.GetSafeErrorMessageWhenInProd(_hostEnvironment.IsDevelopment()));
        }
    }

    protected abstract Task<HandlerResult<TCommandResult>> InternalHandleAsync(TCommand command, CancellationToken cancellationToken);
}