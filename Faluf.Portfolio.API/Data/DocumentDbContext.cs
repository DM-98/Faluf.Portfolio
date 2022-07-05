using Faluf.Portfolio.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Faluf.Portfolio.API.Data;

public class DocumentDbContext : DbContext
{
    public DbSet<Document>? Documents { get; set; }

    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options) { }
}