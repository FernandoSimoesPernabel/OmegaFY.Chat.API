using Microsoft.AspNetCore.Identity;
using OmegaFY.Chat.API.Infra.Authentication.Models;

namespace OmegaFY.Chat.API.Infra.Authentication.Services;

public interface IUserManager
{
    public Task<SignInResult> PasswordSignInAsync(LoginInput loginInput, CancellationToken cancellationToken);

    public Task<IdentityResult> CreateAsync(LoginInput loginInput, CancellationToken cancellationToken);

    public Task<IdentityUser<Guid>> FindByEmailAsync(string email, CancellationToken cancellationToken);
}
