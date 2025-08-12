using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Infra.Authentication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OmegaFY.Chat.API.Infra.Authentication.Services.Implementations;

internal sealed class JwtSecurityTokenProvider : IJwtProvider
{
    private readonly JwtSettings _jwtSettings;

    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtSecurityTokenProvider(IOptions<JwtSettings> options, TokenValidationParameters tokenValidationParameters)
    {
        _jwtSettings = options.Value;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public AuthenticationToken RefreshToken(AuthenticationToken currentToken, RefreshTokenInput refreshTokenInput)
    {
        ClaimsPrincipal claims = new JwtSecurityTokenHandler().ValidateToken(currentToken.Token, _tokenValidationParameters, out SecurityToken securityToken);

        if (claims is null || securityToken is not JwtSecurityToken jwtSecurityToken)
            throw new UnauthorizedException();

        if (jwtSecurityToken.ValidTo < DateTime.UtcNow || jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256Signature)
            throw new UnauthorizedException();

        if (currentToken.RefreshTokenExpirationDate < DateTime.UtcNow)
            throw new UnauthorizedException();

        if (jwtSecurityToken.Issuer != _jwtSettings.Issuer || !jwtSecurityToken.Audiences.Contains(_jwtSettings.Audience))
            throw new UnauthorizedException();

        return WriteToken(refreshTokenInput.UserId, refreshTokenInput.Email, refreshTokenInput.UserName);
    }

    public AuthenticationToken WriteToken(LoginInput loginInput) => WriteToken(loginInput.UserId, loginInput.Email, loginInput.UserName);

    public AuthenticationToken WriteToken(Guid userId, string email, string username)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        long issuedAt = utcNow.ToUnixTimeSeconds();

        long expiresInUnixTimeInSeconds = utcNow.Add(_jwtSettings.TimeToExpireToken).ToUnixTimeSeconds();

        DateTime tokenExpiresIn = DateTimeOffset.FromUnixTimeSeconds(expiresInUnixTimeInSeconds).UtcDateTime;

        DateTime refreshExpiresIn = tokenExpiresIn.Add(_jwtSettings.TimeToExpireRefreshToken);

        Claim[] userClaims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString()),
            new Claim(JwtRegisteredClaimNames.Nbf, issuedAt.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken token = new JwtSecurityToken(
            audience: _jwtSettings.Audience,
            issuer: _jwtSettings.Issuer,
            claims: userClaims,
            expires: tokenExpiresIn,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));

        return new AuthenticationToken(new JwtSecurityTokenHandler().WriteToken(token), tokenExpiresIn, refreshExpiresIn);
    }
}