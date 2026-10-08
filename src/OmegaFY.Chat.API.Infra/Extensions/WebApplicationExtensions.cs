using Microsoft.AspNetCore.Builder;
using OmegaFY.Chat.API.Infra.Constants;
using OmegaFY.Chat.API.Infra.Hubs.Implementations;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapSignalHub(this WebApplication app)
    {
        app.MapHub<ChatNotificationHub>(SignalRHubConstants.HUB_PATH);
        return app;
    }
}