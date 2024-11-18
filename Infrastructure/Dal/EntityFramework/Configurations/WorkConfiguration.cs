using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности Work для базы данных
/// </summary>
public class WorkConfiguration : IEntityTypeConfiguration<Work>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<Work> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasColumnName("title");

        builder.Property(p => p.Description)
            .HasColumnName("description");

        builder.Property(p => p.PublicationDate)
            .IsRequired()
            .HasColumnName("publication_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone");
        
        builder.Property(p => p.CoverUrl)
            .IsRequired()
            .HasColumnName("cover_url");

        builder.Property(p => p.Category)
            .IsRequired()
            .HasDefaultValue(Category.None)
            .HasColumnName("category");
        
        builder.Property(p => p.Likes)
            .IsRequired()
            .HasColumnName("likes");
        
        builder.Property(p => p.Views)
            .IsRequired()
            .HasColumnName("views");

        builder.HasOne<User>(p => p.User)
            .WithMany(p => p.Works)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Chapters)
            .WithOne()
            .HasForeignKey(p => p.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}