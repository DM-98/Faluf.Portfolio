using Faluf.Portfolio.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Faluf.Portfolio.API.Data;

public class SubjectDbContext : DbContext
{
    public DbSet<Subject>? Subjects { get; set; }

    public SubjectDbContext(DbContextOptions<SubjectDbContext> options) : base(options) { }
}