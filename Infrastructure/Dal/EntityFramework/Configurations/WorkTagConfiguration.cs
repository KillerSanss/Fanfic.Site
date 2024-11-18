using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности WorkTag для базы данных
/// </summary>
public class WorkTagConfiguration : IEntityTypeConfiguration<WorkTag>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<WorkTag> builder)
    {
        builder.HasKey(p => new { p.WorkId, p.TagId });
        
        builder.Property(p => p.WorkId)
            .HasColumnName("work_id");
        
        builder.Property(p => p.TagId)
            .HasColumnName("tag_id");

        builder.HasOne<Work>(p => p.Work)
            .WithMany(p => p.WorkTags)
            .HasForeignKey(p => p.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Tag>(p => p.Tag)
            .WithMany(p => p.WorkTags)
            .HasForeignKey(p => p.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}