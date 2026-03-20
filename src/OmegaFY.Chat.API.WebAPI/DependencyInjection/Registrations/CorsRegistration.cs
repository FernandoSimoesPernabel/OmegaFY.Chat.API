namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class CorsRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                //TODO fazer isso melhor na branch do CORS, talvez ler as origins de um arquivo de configuração ou algo do tipo
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
    }
}