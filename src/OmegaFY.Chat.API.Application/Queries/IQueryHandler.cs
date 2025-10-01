namespace OmegaFY.Chat.API.Application.Queries;

public interface IQueryHandler<TQuery, TQueryResult> where TQuery : IQuery where TQueryResult : IQueryResult
{
}