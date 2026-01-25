using Microsoft.AspNetCore.Http;
using NSubstitute;
using OmegaFY.Chat.API.Infra.Cache;
using OmegaFY.Chat.API.Infra.Cache.Models;
using OmegaFY.Chat.API.WebAPI.Middlewares;

namespace OmegaFY.Chat.API.Tests.Unit.WebAPI.Middlewares;

public class HttpRequestIdempotencyMiddlewareFacts
{
    private readonly IHybridCacheProvider _hybridCacheProvider;
    private readonly HttpRequestIdempotencyMiddleware _sut;

    public HttpRequestIdempotencyMiddlewareFacts()
    {
        _hybridCacheProvider = Substitute.For<IHybridCacheProvider>();
        _sut = new HttpRequestIdempotencyMiddleware(_hybridCacheProvider);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenRequestMethodIsGet()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;
        var nextCalled = false;
        RequestDelegate next = _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.True(nextCalled);
        await _hybridCacheProvider.DidNotReceive().GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Any<CacheOptions>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400_WhenIdempotencyKeyHeaderIsMissing()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400_WhenIdempotencyKeyHeaderIsEmpty()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers["Idempotency-Key"] = "";
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400_WhenIdempotencyKeyHeaderIsWhitespace()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers["Idempotency-Key"] = "   ";
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn409_WhenIdempotencyKeyAlreadyExists()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers["Idempotency-Key"] = "test-key-123";
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        _hybridCacheProvider.GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Any<CacheOptions>(),
            Arg.Any<CancellationToken>())
            .Returns((true, true));

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenIdempotencyKeyIsNewAndValid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers["Idempotency-Key"] = "test-key-456";
        context.Response.Body = new MemoryStream();
        var nextCalled = false;
        RequestDelegate next = _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        _hybridCacheProvider.GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Any<CacheOptions>(),
            Arg.Any<CancellationToken>())
            .Returns((false, true));

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ShouldStoreKeyWithCorrectCacheKey_WhenIdempotencyKeyIsProvided()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Put;
        string idempotencyKey = "unique-key-789";
        context.Request.Headers["Idempotency-Key"] = idempotencyKey;
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        _hybridCacheProvider.GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Any<CacheOptions>(),
            Arg.Any<CancellationToken>())
            .Returns((false, true));

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        await _hybridCacheProvider.Received(1).GetOrCreateAsync<bool>(
            Arg.Is<string>(k => k == $"idempotency:{idempotencyKey}"),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Is<CacheOptions>(opts => opts.Expiration == TimeSpan.FromMinutes(1)),
            Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    public async Task InvokeAsync_ShouldCheckIdempotencyKey_ForNonGetMethods(string httpMethod)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = httpMethod;
        context.Request.Headers["Idempotency-Key"] = "test-key";
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        _hybridCacheProvider.GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Any<CacheOptions>(),
            Arg.Any<CancellationToken>())
            .Returns((false, true));

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        await _hybridCacheProvider.Received(1).GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Any<CacheOptions>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvokeAsync_ShouldUseOneMinuteExpiration_WhenStoringIdempotencyKey()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Headers["Idempotency-Key"] = "test-key";
        context.Response.Body = new MemoryStream();
        RequestDelegate next = _ => Task.CompletedTask;

        CacheOptions capturedOptions = null;
        _hybridCacheProvider.GetOrCreateAsync<bool>(
            Arg.Any<string>(),
            Arg.Any<Func<CancellationToken, ValueTask<bool>>>(),
            Arg.Do<CacheOptions>(opts => capturedOptions = opts),
            Arg.Any<CancellationToken>())
            .Returns((false, true));

        // Act
        await _sut.InvokeAsync(context, next);

        // Assert
        Assert.NotNull(capturedOptions);
        Assert.Equal(TimeSpan.FromMinutes(1), capturedOptions.Expiration);
    }
}
