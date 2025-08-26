namespace OmegaFY.Chat.API.WebAPI.Controllers.Base;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
//[Authorize(PoliciesNamesConstants.BEARER_JWT_POLICY)]
[Route("api/[controller]/")]
public abstract class ApiControllerBase : ControllerBase
{
}