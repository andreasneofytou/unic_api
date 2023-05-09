using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UnicApi.Users.Entities;

namespace UnicApi.Data;

public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, string>
{
    public DbSet<LecturerEntity> Lecturers { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
               : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserEntity>().ToTable("User");
        builder.Entity<UserEntity>(entity => entity.Property(e => e.Id).ValueGeneratedOnAdd());

        builder.Entity<RoleEntity>().ToTable("Role");
        builder.Entity<RoleEntity>(entity => entity.Property(e => e.Id).ValueGeneratedOnAdd());

        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

        builder.Entity<LecturerEntity>().ToTable("Lecturer");


        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}