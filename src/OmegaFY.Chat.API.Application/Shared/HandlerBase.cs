using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using OmegaFY.Chat.API.Application.Extensions;
using OmegaFY.Chat.API.Common.Exceptions.Base;
using OmegaFY.Chat.API.Common.Exceptions.Constants;
using OmegaFY.Chat.API.Common.Extensions;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.OpenTelemetry.Providers;
using System.Diagnostics;

namespace OmegaFY.Chat.API.Application.Shared;

public abstract class HandlerBase<THandler, TRequest, TResult> where TRequest : IRequest where TResult : IResult
{
    protected readonly IHostEnvironment _hostEnvironment;

    protected readonly IOpenTelemetryRegisterProvider _openTelemetryRegisterProvider;

    protected readonly IValidator<TRequest> _validator;

    protected HandlerBase(
        IHostEnvironment hostEnvironment,
        IOpenTelemetryRegisterProvider openTelemetryRegisterProvider,
        IValidator<TRequest> validator)
    {
        _hostEnvironment = hostEnvironment;
        _openTelemetryRegisterProvider = openTelemetryRegisterProvider;
        _validator = validator;
    }

    public async Task<HandlerResult<TResult>> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        using Activity activity = _openTelemetryRegisterProvider.StartActivity(OpenTelemetryConstants.ACTIVITY_APPLICATION_HANDLER_NAME);
        activity.SetHandlerName(GetType().Name);

        try
        {
            activity.SetRequest(request);

            ValidationResult validationResult = _validator.Validate(request);

            HandlerResult<TResult> result = 
                validationResult.IsValid ? await InternalHandleAsync(request, cancellationToken) : validationResult.ToHandlerResult<TResult>();

            activity.SetResult(result);

            return result;
        }
        catch (ErrorCodeException ex)
        {
            activity.SetResult(ex);
            return new HandlerResult<TResult>(ex.ErrorCode, ex.Message);
        }
        catch (Exception ex)
        {
            activity.SetResult(ex);
            return new HandlerResult<TResult>(ApplicationErrorCodesConstants.NOT_DOMAIN_ERROR, ex.GetSafeErrorMessageWhenInProd(_hostEnvironment.IsDevelopment()));
        }
    }

    protected abstract Task<HandlerResult<TResult>> InternalHandleAsync(TRequest request, CancellationToken cancellationToken);
}