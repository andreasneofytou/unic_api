using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnicApi.Data;
using UnicApi.Users.Entities;
using UnicApi.Users.Models;

namespace UnicApi.Users;

public class UsersService
{
    private readonly AppDbContext appDbContext;
    private readonly UserManager<UserEntity> userManager;

    private DbSet<LecturerEntity> lecturers;

    public UsersService(UserManager<UserEntity> userManager, AppDbContext appDbContext)
    {
        this.userManager = userManager;
        this.appDbContext = appDbContext;
        lecturers = appDbContext.Set<LecturerEntity>();
    }

    public async Task<UserEntity> CreateUserAsync(CreateUserModel createUserModel)
    {
        var user = new UserEntity
        {
            FirstName = createUserModel.FirstName,
            LastName = createUserModel.LastName,
            UserName = createUserModel.Email,
            Email = createUserModel.Email
        };
        IdentityResult result = await userManager.CreateAsync(user, createUserModel.Password);

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

        return user;
    }

    public async Task<LecturerEntity?> CreateLecturerAsync(CreateLecturerModel createLecturerModel)
    {
        var user = await CreateUserAsync(new CreateUserModel
        {
            FirstName = createLecturerModel.FirstName,
            LastName = createLecturerModel.LastName,
            Email = createLecturerModel.Email
        });

        if (user != null)
        {
            var lecturer = new LecturerEntity { Id = user.Id, SocialInsuranceNumber = createLecturerModel.SocialInsuranceNumber };
            await lecturers.AddAsync(lecturer);
            await appDbContext.SaveChangesAsync();

            return lecturer;
        }

        return null;
    }

    public async Task<UserEntity?> FindByEmailAsync(string email) => await userManager.FindByEmailAsync(email);

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password) => await userManager.CheckPasswordAsync(user, password);

}