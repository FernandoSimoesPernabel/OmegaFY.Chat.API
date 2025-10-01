using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Data.EF.Repositories.Base;
using OmegaFY.Chat.API.Data.EF.Repositories.CompiledQueries;
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

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<User> GetByIdAsync(ReferenceId id, CancellationToken cancellationToken) => await _dbSet.FindAsync([id], cancellationToken);

    public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => UserRepositoryCompiledQueries.GetByEmailAsync(_context, email, cancellationToken)!;

    public Task<bool> CheckIfUserAlreadyExistsAsync(string email, CancellationToken cancellationToken)
        => UserRepositoryCompiledQueries.CheckIfUserAlreadyExistsAsync(_context, email, cancellationToken);
}