using Microsoft.AspNetCore.Identity;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using OmegaFY.Chat.API.Infra.Authentication.Services;

namespace OmegaFY.Chat.API.Data.EF.Authentication.Services;

internal sealed class EntityFrameworkUserManager : IUserManager
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;


    private readonly SignInManager<IdentityUser<Guid>> _signInManager;

    public EntityFrameworkUserManager(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> PasswordSignInAsync(LoginInput loginInput, CancellationToken cancellationToken)
    {
        IdentityUser<Guid> identityUser = await _userManager.FindByEmailAsync(loginInput.Email);

        if (identityUser is null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(identityUser, loginInput.Password, isPersistent: false, lockoutOnFailure: true);
    }

    public Task<IdentityResult> CreateAsync(LoginInput loginInput, CancellationToken cancellationToken)
    {
        IdentityUser<Guid> identityUser = new IdentityUser<Guid>()
        {
            Id = loginInput.UserId,
            Email = loginInput.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = loginInput.Email
        };

        return _userManager.CreateAsync(identityUser, loginInput.Password);
    }

    public Task<IdentityUser<Guid>> FindByEmailAsync(string email, CancellationToken cancellationToken) => _userManager.FindByEmailAsync(email);
}
