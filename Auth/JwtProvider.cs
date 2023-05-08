using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UnicApi.Users.Entities;

namespace UnicApi.Auth;

public class JwtProvider
{
    public string GenerateAccessToken(UserEntity user)
    {
        var key = "mylongkeymylongkeymylongkeymylongkeymylongkey";
        var encodedKey = Encoding.UTF8.GetBytes(key);
        var symmetricKey = new SymmetricSecurityKey(encodedKey);
        var signedCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = "localhost",
            SigningCredentials = signedCredentials,
            Audience = "localhost",

        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenJwt = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(tokenJwt);

        return token;
    }
}