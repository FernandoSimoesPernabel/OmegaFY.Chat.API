using Microsoft.AspNetCore.Http;
using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Infra.Authentication.Extensions;

namespace OmegaFY.Chat.API.Infra.Authentication.Users.Implementations;

internal sealed class HttpContextAccessorUserInformation : IUserInformation
{
    public Guid? CurrentRequestUserId { get; }

    public string Email { get; }

    public HttpContextAccessorUserInformation(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext.User.IsAuthenticated())
        {
            Guid? userId = httpContextAccessor.HttpContext.User.TryGetUserIdFromClaims();

            if (userId is null)
                throw new UnauthorizedException();

            string email = httpContextAccessor.HttpContext.User.TryGetEmailFromClaims();

            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedException();

            CurrentRequestUserId = userId;
            Email = email;
        }
    }
}