using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using OmegaFY.Chat.API.Infra.Hubs.Implementations;

namespace OmegaFY.Chat.API.Infra.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapSignalHub(this WebApplication app)
    {
        app.MapHub<ChatNotificationHub>("/hub");
        return app;
    }
}