using OmegaFY.Chat.API.Application.Bus;
using OmegaFY.Chat.API.WebAPI.Controllers.Base;

namespace OmegaFY.Chat.API.WebAPI.Controllers;

public sealed class AuthController : ApiControllerBase
{
    public AuthController(IServiceBus serviceBus) : base(serviceBus) { }
}