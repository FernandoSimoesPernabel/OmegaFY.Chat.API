
using OmegaFY.Chat.API.WebAPI.FIlters;
using System.Text.Json.Serialization;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

internal sealed class WebApiRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(controllerOptions =>
        {
            controllerOptions.Filters.Add<ApiResponseActionFilter>();
            controllerOptions.Filters.Add<OpenTelemetryInstrumentationFilter>();

            controllerOptions.SuppressAsyncSuffixInActionNames = true;
        })
        .AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}