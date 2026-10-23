namespace OmegaFY.Chat.API.Infra.IA;

public interface IAgent<in TRequest, TResult> where TRequest : class where TResult : class
{
    public Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}