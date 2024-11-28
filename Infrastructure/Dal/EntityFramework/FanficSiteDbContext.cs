using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.EntityFramework;

/// <summary>
/// Контекст базы данных
/// </summary>
public class FanficSiteDbContext : DbContext
{
    /// <summary>
    /// Работы
    /// </summary>
    public DbSet<Work> Works { get; init; }
    
    /// <summary>
    /// Тэги
    /// </summary>
    public DbSet<Tag> Tags { get; init; }
    
    /// <summary>
    /// Комментарии
    /// </summary>
    public DbSet<Comment> Comments { get; init; }
    
    /// <summary>
    /// Главы
    /// </summary>
    public DbSet<Chapter> Chapters { get; init; }
    
    /// <summary>
    /// Пользователи
    /// </summary>
    public DbSet<User> Users { get; init; }
    
    /// <summary>
    /// WorkTags
    /// </summary>
    public DbSet<WorkTag> WorkTags { get; init; }
    
    /// <summary>
    /// WorkLikes
    /// </summary>
    public DbSet<WorkLike> WorkLikes { get; init; }
    
    /// <summary>
    /// UserTags
    /// </summary>
    public DbSet<UserTag> UserTags { get; init; }

    public FanficSiteDbContext(DbContextOptions<FanficSiteDbContext> options) : base(options)
    {
    }

    public FanficSiteDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(
            "User ID=postgres;Password=sans;Host=fanficsite_db;Port=5432;Database=FanficSite;");
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