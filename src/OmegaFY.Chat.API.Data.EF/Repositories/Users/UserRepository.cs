using Microsoft.EntityFrameworkCore;
using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Repositories.Base;
using OmegaFY.Chat.API.Domain.Entities.Users;
using OmegaFY.Chat.API.Domain.Repositories.Users;
using OmegaFY.Chat.API.Domain.ValueObjects.Shared;

namespace OmegaFY.Chat.API.Data.EF.Repositories.Users;

internal sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationContext applicationContext) : base(applicationContext) { }

    public Task CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        _dbSet.Add(user);
        return Task.CompletedTask;
    }

    public async Task<User> GetByIdAsync(ReferenceId id, CancellationToken cancellationToken) => await _dbSet.FindAsync([id], cancellationToken);

    public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => _dbSet.Where(user => user.Email == email).FirstOrDefaultAsync(cancellationToken);

    public Task<bool> CheckIfUserAlreadyExistsAsync(string email, CancellationToken cancellationToken)
        => _dbSet.Where(user => user.Email == email).AsNoTracking().AnyAsync(cancellationToken);
}