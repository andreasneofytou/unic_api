using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicApi.Users.Entities;

namespace UnicApi.Classes;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClassesController : ControllerBase
{
    private readonly ClassesService classesService;

    public ClassesController(ClassesService classesService)
    {
        this.classesService = classesService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? periodId = null)
    {
        string? role = null;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            role = identity.FindFirst(ClaimTypes.Role)?.Value;
        }

        var classes = await classesService.GetClassesAsync(userId: null, periodId: periodId);

        // Replace this with automapper
        return Ok(classes.Select(c => new
        {
            c.Name,
            PeriodName = c.Period.Name,
            Students = role == RolesEnum.Lecturer.ToString() ? c.Students?.Count
        : -1
        }));
    }

    [HttpGet("my-classes")]
    public async Task<IActionResult> GetUserClasses([FromQuery] string? periodId = null)
    {
        string? userId = null;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            userId = identity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        }

        var classes = await classesService.GetClassesAsync(userId: userId, periodId: periodId);

        // Replace this with automapper
        return Ok(classes.Select(c => new { c.Name, PeriodName = c.Period.Name, Students = c.Students?.Count ?? -1 }));
    }
}