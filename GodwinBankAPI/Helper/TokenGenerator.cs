using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GodwinBankAPI.Configurations;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.Response;
using Microsoft.IdentityModel.Tokens;

namespace GodwinBankAPI.Helper;

public static class TokenGenerator
{
    public static TokenResponse GenerateToken(this AccountResponseModel accountResponseModel, BearerTokenConfig config)
    {
        if (accountResponseModel is null)
            throw new ArgumentNullException(nameof(accountResponseModel), "Account must not be null or empty");

        if (config is null)
            throw new ArgumentNullException(nameof(config), "Bearer token configuration must not be null or empty");

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Key));
        var now = DateTime.UtcNow;

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, accountResponseModel.EmailAddress)
        };

        var token = new JwtSecurityToken(
            config.Issuer,
            config.Audience,
            claims,
            now.AddMilliseconds(0),
            now.AddHours(12),
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        );

        string tokenString = tokenHandler.WriteToken(token);

        return new TokenResponse()
        {
            BearerToken = tokenString,
            Expiry = token.Payload.Exp
        };
        
    }
}