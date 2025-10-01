namespace OmegaFY.Chat.API.Application.Shared;

public record class HandlerResult<TResult> : HandlerResult
{
    public TResult Data { get; init; }

    public HandlerResult() : base() { }

    public HandlerResult(string code, string message) : base(code, message) { }

    public HandlerResult(TResult data) => Data = data;
}