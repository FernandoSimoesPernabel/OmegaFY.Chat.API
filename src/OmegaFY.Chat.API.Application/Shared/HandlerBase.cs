using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Extensions;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Application.Shared;

public abstract class HandlerBase<THandler, TRequest, TResult> where TRequest : IRequest where TResult : IResult
{
    protected readonly IHostEnvironment _hostEnvironment;

    protected readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    protected readonly IValidator<TRequest> _validator;

    protected readonly ILogger<THandler> _logger;

    protected HandlerBase(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<TRequest> validator,
        ILogger<THandler> logger)
    {
        _hostEnvironment = hostEnvironment;
        _openTelemetryRegisterProvider = openTelemetryRegisterProvider;
        _validator = validator;
        _logger = logger;
    }

    public async Task<HandlerResult<TResult>> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_APPLICATION_HANDLER_NAME);
        activity.SetHandlerName(typeof(THandler).Name);

        try
        {
            _logger.LogInformation("Handling request {HandlerType} with correlation {CorrelationId}", typeof(THandler).Name, activity.Id);

            activity.SetRequest(request);

            ValidationResult validationResult = _validator.Validate(request);

            HandlerResult<TResult> result =
                validationResult.IsValid ? await InternalHandleAsync(request, cancellationToken) : validationResult.ToHandlerResult<TResult>();

            activity.SetResult(result);

            if (!result.Succeeded())
                _logger.LogWarning("Handling completed with errors in {HandlerType}: {Errors}", typeof(THandler).Name, result.GetErrorsAsStringSeparatedByNewLine());

            _logger.LogInformation("Handling finished in {HandlerType}", typeof(THandler).Name);

            return result;
        }
        catch (ErrorCodeException ex)
        {
            activity.SetErrorStatus(ex);
            
            _logger.LogWarning(ex, "Domain error in {HandlerType}: {Code} - {Message}", typeof(THandler).Name, ex.ErrorCode, ex.Message);
            
            return new HandlerResult<TResult>(ex.ErrorCode, ex.Message);
        }
        catch (Exception ex)
        {
            activity.SetErrorStatus(ex);
            
            _logger.LogError(ex, "Unexpected error in {HandlerType}", typeof(THandler).Name);
            
            return new HandlerResult<TResult>(ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR, ex.GetSafeErrorMessageWhenInProd(_hostEnvironment.IsDevelopment()));
        }
    }

    protected abstract Task<HandlerResult<TResult>> InternalHandleAsync(TRequest request, CancellationToken cancellationToken);
}