using Microsoft.AspNetCore.Authorization;
using OmegaFY.Chat.API.Infra.Authentication.Constants;

namespace OmegaFY.Chat.API.WebAPI.Controllers.Base;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Authorize(PoliciesNamesConstants.BEARER_JWT_POLICY)]
[Route("api/[controller]/")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly IServiceBus _serviceBus;

    public ApiControllerBase(IServiceBus serviceBus) => _serviceBus = serviceBus;
}