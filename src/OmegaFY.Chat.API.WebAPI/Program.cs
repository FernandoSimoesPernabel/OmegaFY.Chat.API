//using HealthChecks.UI.Client;
//using Microsoft.AspNetCore.Diagnostics.HealthChecks;
//using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Data.EF.Extensions;
using OmegaFY.Chat.API.WebAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencyInjectionRegister(builder);

WebApplication app = builder.Build();

await app.RunMigrationsAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options => options.UseRequestInterceptor("(request) => { request.headers['Idempotency-Key'] = crypto.randomUUID(); return request; }"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpRequestIdempotencyMiddleware();

//app.UseHealthChecks(HealthCheckConstants.API_ENDPOINT, new HealthCheckOptions()
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

//app.UseHealthChecksUI(options => options.UIPath = HealthCheckConstants.UI_ENDPOINT);

app.UseRateLimiter();

app.MapControllers();

await app.RunAsync();