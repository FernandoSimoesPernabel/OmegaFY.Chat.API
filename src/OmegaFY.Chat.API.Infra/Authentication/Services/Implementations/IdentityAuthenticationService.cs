using Microsoft.AspNetCore.Identity;
using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Infra.Authentication.Extensions;
using OmegaFY.Chat.API.Infra.Authentication.Models;

namespace OmegaFY.Chat.API.Infra.Authentication.Services.Implementations;

internal sealed class IdentityAuthenticationService : IAuthenticationService
{
    private readonly IUserManager _userManager;

    private readonly IJwtProvider _jwtProvider;

    public IdentityAuthenticationService(IUserManager userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthenticationToken> RegisterNewUserAsync(LoginInput loginInput, CancellationToken cancellationToken)
    {
        bool userAlreadyRegister = await _userManager.FindByEmailAsync(loginInput.Email, cancellationToken) is not null;

        if (userAlreadyRegister)
            throw new ConflictedException();

        IdentityResult createUserResult = await _userManager.CreateAsync(loginInput, cancellationToken);

        if (!createUserResult.Succeeded)
            throw new UnableToCreateUserOnIdentityException();

        return await LoginAsync(loginInput, cancellationToken);
    }

    public async Task<AuthenticationToken> LoginAsync(LoginInput loginInput, CancellationToken cancellationToken)
    {
        SignInResult signInResult = await _userManager.PasswordSignInAsync(loginInput, cancellationToken);

        signInResult.EnsureSuccessStatus();

        return _jwtProvider.WriteToken(loginInput);
    }

    public async Task<AuthenticationToken> RefreshTokenAsync(AuthenticationToken currentToken, RefreshTokenInput refreshTokenInput, CancellationToken cancellationToken)
    {
        IdentityUser<Guid> identityUser = await _userManager.FindByEmailAsync(refreshTokenInput.Email, cancellationToken);

        if (identityUser is null)
            throw new UnauthorizedException();

        return _jwtProvider.RefreshToken(currentToken, refreshTokenInput);
    }
}