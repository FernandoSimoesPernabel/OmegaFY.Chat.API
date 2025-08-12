using OmegaFY.Chat.API.Infra.Authentication.Models;

namespace OmegaFY.Chat.API.Infra.Authentication.Services;

internal interface IJwtProvider
{
    public AuthenticationToken RefreshToken(AuthenticationToken currentToken, RefreshTokenInput refreshTokenInput);

    public AuthenticationToken WriteToken(LoginInput loginOptions);

    public AuthenticationToken WriteToken(Guid userId, string email, string username);
}