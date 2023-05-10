using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UnicApi.Auth;
using UnicApi.Data;
using UnicApi.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UnicApi.Auth.Options;
using UnicApi.Classes;
using UnicApi.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return new UnprocessableEntityObjectResult(context.ModelState);
    };
});
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"])
            );
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? string.Empty)),
        ValidateIssuer = true,
        ValidIssuer = "localhost",
        ValidateAudience = true,
        ValidAudience = "localhost",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

//Services
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<JwtProvider>();
builder.Services.AddTransient<UsersService>();
builder.Services.AddTransient<ClassesService>();


var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    SeedData.Initialise(services);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
