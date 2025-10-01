using OmegaFY.Chat.API.Domain.Entities;

namespace OmegaFY.Chat.API.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot<TEntity>
{
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}