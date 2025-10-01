using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace OmegaFY.Chat.API.WebAPI.DependencyInjection.Registrations;

public sealed class ApiResponseCompressionRegistration : IDependencyInjectionRegister
{
    public void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;

            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);

        builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);
    }
}