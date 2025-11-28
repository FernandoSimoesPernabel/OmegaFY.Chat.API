
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Common.Models;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class SwaggerOpenApiRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Contact = new OpenApiContact()
                {
                    Name = "Fernando Simões Pernabel",
                    Url = new Uri("https://github.com/FernandoSimoesPernabel/OmegaFY.Chat.API"),
                    Email = "f_pernabel@hotmail.com"
                },
                Description = "For more information access https://github.com/FernandoSimoesPernabel/OmegaFY.Chat.API",
                Title = ApplicationInfoConstants.APPLICATION_NAME,
                License = new OpenApiLicense()
                {
                    Name = "GNU General Public License version 3",
                    Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.html")
                },
                Version = ProjectVersion.Instance.ToString()
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: Authorization: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
            });
        });
    }
}