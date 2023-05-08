using Microsoft.AspNetCore.Identity;
using UnicApi.Auth.Models;
using UnicApi.Users.Entities;

namespace UnicApi.Auth;

public class AuthService
{
    private readonly JwtProvider jwtProvider;
    private readonly UserManager<UserEntity> userManager;

    public AuthService(JwtProvider jwtProvider, UserManager<UserEntity> userManager)
    {
        this.jwtProvider = jwtProvider;
        this.userManager = userManager;
    }

    public async Task<string> SignInAsync(SignInModel signInModel)
    {

        var user = await userManager.FindByEmailAsync(signInModel.Username);
        if (user != null)
        {
            bool isPassCorrect = await userManager.CheckPasswordAsync(user, signInModel.Password);
            if (isPassCorrect)
            {
                var token = jwtProvider.GenerateAccessToken(user);
                return token;
            }
        }

        return string.Empty;
    }

    public async Task<string> RegisterAsync(RegisterModel registerModel)
    {
        var user = new UserEntity { FirstName = registerModel.FirstName, LastName = registerModel.LastName, UserName = registerModel.Email, Email = registerModel.Email };
        IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);

        if (!result.Succeeded)
        {
            throw new BadHttpRequestException("User couldn't be created");
        }

        return jwtProvider.GenerateAccessToken(user);
    }
}