using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности Chapter для базы данных
/// </summary>
public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder.Property(p => p.WorkId)
            .IsRequired()
            .HasColumnName("work_id");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasColumnName("title");

        builder.Property(p => p.Description)
            .HasColumnName("description");
        
        builder.Property(p => p.Content)
            .IsRequired()
            .HasColumnName("content");
        
        builder.HasOne<User>(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Work>(p => p.Work)
            .WithMany(p => p.Chapters)
            .HasForeignKey(p => p.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}