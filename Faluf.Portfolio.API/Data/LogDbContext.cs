using Faluf.Portfolio.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Faluf.Portfolio.API.Data;

public class LogDbContext : DbContext
{
    public DbSet<Log>? Logs { get; set; }

    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }
}