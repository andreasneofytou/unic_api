using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicApi.Users.Entities;
using UnicApi.Users.Models;

namespace UnicApi.Users;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{
    private readonly UsersService usersService;

    public UsersController(UsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost("lecturer")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateLecturer(CreateLecturerModel createLecturerModel)
    {
        LecturerEntity? user = await usersService.CreateLecturerAsync(createLecturerModel);
        return Ok(user);
    }
}