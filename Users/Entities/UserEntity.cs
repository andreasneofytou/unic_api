using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UnicApi.Users.Entities;

public class UserEntity : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}