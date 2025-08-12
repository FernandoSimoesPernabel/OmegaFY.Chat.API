using OmegaFY.Chat.API.Infra.Authentication.Models;

namespace OmegaFY.Chat.API.Infra.Authentication.Services;

public interface IAuthenticationService
{
    public Task<AuthenticationToken> RefreshTokenAsync(AuthenticationToken currentToken, RefreshTokenInput refreshTokenInput, CancellationToken cancellationToken);

    public Task<AuthenticationToken> RegisterNewUserAsync(LoginInput loginInput, CancellationToken cancellationToken);

    public Task<AuthenticationToken> LoginAsync(LoginInput loginInput, CancellationToken cancellationToken);
}