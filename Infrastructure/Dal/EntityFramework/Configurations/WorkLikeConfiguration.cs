using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности WorkLike для базы данных
/// </summary>
public class WorkLikeConfiguration : IEntityTypeConfiguration<WorkLike>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<WorkLike> builder)
    {
        builder.HasKey(p => new { p.WorkId, p.UserId });
        
        builder.Property(p => p.WorkId)
            .HasColumnName("work_id");
        
        builder.Property(p => p.UserId)
            .HasColumnName("user_id");

        builder.HasOne<Work>(p => p.Work)
            .WithMany(p => p.WorkLikes)
            .HasForeignKey(p => p.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<User>(p => p.User)
            .WithMany(p => p.WorkLikes)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}