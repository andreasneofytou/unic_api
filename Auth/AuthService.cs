using System.Text;
using Microsoft.AspNetCore.Identity;
using UnicApi.Auth.Models;
using UnicApi.Users;
using UnicApi.Users.Entities;
using UnicApi.Users.Models;

namespace UnicApi.Auth;

public class AuthService
{
    private readonly JwtProvider jwtProvider;
    private readonly UsersService usersService;

    public AuthService(JwtProvider jwtProvider, UsersService usersService)
    {
        this.jwtProvider = jwtProvider;
        this.usersService = usersService;
    }

    public async Task<string> SignInAsync(SignInModel signInModel)
    {
        var user = await usersService.FindByEmailAsync(signInModel.Username);
        if (user != null)
        {
            bool isPassCorrect = await usersService.CheckPasswordAsync(user, signInModel.Password);
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
        var user = await usersService.CreateUserAsync(new CreateUserModel
        {
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Email = registerModel.Email
        });

        return await jwtProvider.GenerateAccessTokenAsync(user);
    }
}