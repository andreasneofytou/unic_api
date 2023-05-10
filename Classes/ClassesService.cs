using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnicApi.Classes.Entities;
using UnicApi.Data;

namespace UnicApi.Classes;

public class ClassesService
{
    private readonly AppDbContext appDbContext;
    private DbSet<SubjectEntity> subjects;

    public ClassesService(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext ?? throw new ArgumentNullException();
        subjects = appDbContext.Set<SubjectEntity>();
    }
    public async Task<IEnumerable<SubjectEntity>> GetClassesAsync(string? userId, string? periodId)
    {
        var q = subjects.Include(p => p.Students).Where(p => (periodId == null || p.Id == periodId) && p.Students.Any(s => userId == null || s.Id == userId));

        return await q.ToListAsync();
    }
}