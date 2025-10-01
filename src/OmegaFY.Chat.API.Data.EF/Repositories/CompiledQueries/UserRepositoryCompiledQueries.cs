using OmegaFY.Chat.API.Data.EF.Context;
using OmegaFY.Chat.API.Domain.Entities.Users;

namespace OmegaFY.Chat.API.Data.EF.Repositories.CompiledQueries;

internal static class UserRepositoryCompiledQueries
{
    public static readonly Func<ApplicationContext, string, CancellationToken, Task<User>> GetByEmailAsync =
        Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((ApplicationContext ctx, string email, CancellationToken ct) =>
            ctx.Set<User>().Where(user => user.Email == email).FirstOrDefault());

    public static readonly Func<ApplicationContext, string, CancellationToken, Task<bool>> CheckIfUserAlreadyExistsAsync =
        Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((ApplicationContext ctx, string email, CancellationToken ct) =>
            ctx.Set<User>().Any(user => user.Email == email));
}