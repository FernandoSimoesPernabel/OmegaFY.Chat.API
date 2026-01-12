using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionRegister(builder);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.UseHealthChecks(HealthCheckConstants.API_ENDPOINT, new HealthCheckOptions()
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

//app.UseHealthChecksUI(options => options.UIPath = HealthCheckConstants.UI_ENDPOINT);

app.UseRateLimiter();

app.MapControllers();

app.Run();