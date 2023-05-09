using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UnicApi.Auth.Options;
using UnicApi.Users.Entities;

namespace UnicApi.Auth;

public class JwtProvider
{
    private readonly JwtOptions jwtOptions;
    private readonly UserManager<UserEntity> userManager;
    private readonly RoleManager<RoleEntity> roleManager;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
    {
        this.jwtOptions = jwtOptions.Value;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }
    public async Task<string> GenerateAccessTokenAsync(UserEntity user)
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
        var signedCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
            var role = await roleManager.FindByNameAsync(userRole);
            if (role != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                foreach (Claim roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
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