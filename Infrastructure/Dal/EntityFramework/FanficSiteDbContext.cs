using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.EntityFramework;

/// <summary>
/// Контекст базы данных
/// </summary>
public class FanficSiteDbContext : DbContext
{
    public DbSet<Work> Works { get; init; }
    public DbSet<Tag> Tags { get; init; }
    public DbSet<Comment> Comments { get; init; }
    public DbSet<Chapter> Chapters { get; init; }
    public DbSet<User> Users { get; init; }
    
    public DbSet<WorkTag> WorkTags { get; init; }
    public DbSet<WorkLike> WorkLikes { get; init; }
    public DbSet<UserTag> UserTags { get; init; }

    public FanficSiteDbContext(DbContextOptions<FanficSiteDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=sans;Host=localhost;Port=5432;Database=FanficSite;");
    }

    /// <summary>
    /// Метод применения конфигураций
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Gender>();
        modelBuilder.HasPostgresEnum<Category>();
        modelBuilder.HasPostgresEnum<Role>();
        modelBuilder.HasPostgresEnum<AgeRestriction>();
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FanficSiteDbContext).Assembly);
    }
}