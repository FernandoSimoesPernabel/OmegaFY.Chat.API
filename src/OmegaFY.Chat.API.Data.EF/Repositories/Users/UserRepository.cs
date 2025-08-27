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

    public async Task CreateUserAsync(User user, CancellationToken cancellationToken) => await _dbSet.AddAsync(user, cancellationToken);

    public async Task<User> GetByIdAsync(ReferenceId id, CancellationToken cancellationToken) => await _dbSet.FindAsync([id], cancellationToken);

    public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken) => _dbSet.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public Task<bool> CheckIfUserAlreadyExistsAsync(string email, CancellationToken cancellationToken) => _dbSet.AnyAsync(user => user.Email == email, cancellationToken);
}
