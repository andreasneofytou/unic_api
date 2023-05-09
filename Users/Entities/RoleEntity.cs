using Microsoft.AspNetCore.Identity;

namespace UnicApi.Users.Entities;

public class RoleEntity : IdentityRole<string>
{
    public RoleEntity() : base()
    {
    }

    public RoleEntity(string name) : base(name)
    {
    }
}

public enum RolesEnum
{
    Superuser,
    Admin,
    Lecturer,
    Student
}