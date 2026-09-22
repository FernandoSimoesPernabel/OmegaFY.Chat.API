using OmegaFY.Chat.API.Infra.Extensions;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class AIAgentsRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddGoogleGeminiChatClients(builder.Configuration);

        builder.Services.AddAIAgents();
    }
}