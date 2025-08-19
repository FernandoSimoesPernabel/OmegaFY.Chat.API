using Microsoft.AspNetCore.Identity;
using OmegaFY.Chat.API.Common.Exceptions;

namespace OmegaFY.Chat.API.Infra.Authentication.Extensions;

public static class SignInResultExtensions
{
    public static void EnsureSuccessStatus(this SignInResult signInResult)
    {
        if (!signInResult.Succeeded)
            throw new UnauthorizedException();

        if (signInResult.IsLockedOut)
            throw new UserLockedOutException();

        if (signInResult.IsNotAllowed)
            throw new UnauthenticatedException();

        if (signInResult.RequiresTwoFactor)
            throw new RequiresTwoFactorException();
    }
}