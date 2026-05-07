using Microsoft.EntityFrameworkCore;
using TinyUrl.Domain.Entities;
using TinyUrl.Infrastructure.Data.Configurations;

namespace TinyUrl.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TinyUrlEntity> TinyUrls => Set<TinyUrlEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TinyUrlConfiguration());
    }
}