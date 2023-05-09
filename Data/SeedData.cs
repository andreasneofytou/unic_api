using Microsoft.AspNetCore.Identity;
using UnicApi.Users.Entities;

namespace UnicApi.Data;

public static class SeedData
{
    private static RoleManager<RoleEntity> roleManager;
    private static UserManager<UserEntity> userManager;

    internal static void Initialise(IServiceProvider provider)
    {
        roleManager = provider.GetService<RoleManager<RoleEntity>>();
        userManager = provider.GetService<UserManager<UserEntity>>();

        CreateRoles();
        CreateUsers();
        var user = userManager.FindByEmailAsync("admin@admin.com").Result;
        if (user == null) return;
        if (roleManager.RoleExistsAsync("Superuser").Result && roleManager.RoleExistsAsync("Admin").Result)
        {
            var res = userManager.AddToRolesAsync(user, new List<string> { "Admin", "Superuser" });
        }
    }

    private static void CreateRoles()
    {
        var roles = new List<string> { "Superuser", "Admin", "Lecturer", "Student" };

        foreach (var role in roles)
        {
            var result = roleManager.RoleExistsAsync(role).Result;
            if (!result)
            {
                RoleEntity newRole = new RoleEntity(role);
                IdentityResult res = roleManager.CreateAsync(newRole).Result;
            }
        }
    }

    private static void CreateUsers()
    {
        var user = new UserEntity
        {
            Email = "admin@admin.com",
            UserName = "admin@admin.com",
            EmailConfirmed = true,
            FirstName = "Andreas",
            LastName = "Neofytou"
        };
        var result = userManager.CreateAsync(user, "indigohome67").Result;
    }
}
