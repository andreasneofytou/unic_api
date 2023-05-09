using System.Text;
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
                var token = await jwtProvider.GenerateAccessTokenAsync(user);
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
            StringBuilder message = new StringBuilder();
            foreach (IdentityError error in result.Errors)
            {
                message.Append(error + Environment.NewLine);
            }
            throw new BadHttpRequestException(message.ToString());
        }

        await userManager.AddToRoleAsync(user, RolesEnum.Student.ToString());

        return await jwtProvider.GenerateAccessTokenAsync(user);
    }
}