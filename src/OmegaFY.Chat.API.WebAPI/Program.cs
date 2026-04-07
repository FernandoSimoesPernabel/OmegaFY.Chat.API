using OmegaFY.Chat.API.Common.Constants;
using OmegaFY.Chat.API.Data.EF.Extensions;
using OmegaFY.Chat.API.Infra.Extensions;
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
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHealthChecks(HealthCheckConstants.API_ENDPOINT);

app.UseRateLimiter();

app.MapControllers();

app.MapSignalHub();

await app.RunAsync();