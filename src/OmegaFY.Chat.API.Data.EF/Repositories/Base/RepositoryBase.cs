using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Domain.Entities;
using OmegaFY.Chat.API.Domain.Repositories;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Base;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot<TEntity>
{
    protected readonly DbContext _context;

    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(DbContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
}