using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Domain.Entities;
using OmegaFY.Chat.API.Domain.Repositories;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Base;

internal abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot<TEntity>
{
    protected readonly ApplicationContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(ApplicationContext applicationContext)
    {
        _context = applicationContext;
        _dbSet = applicationContext.Set<TEntity>();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
}