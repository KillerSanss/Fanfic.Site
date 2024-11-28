using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности Tag для базы данных
/// </summary>
public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnName("name");
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnName("description");
        
        builder.Property(p => p.AgeRestriction)
            .IsRequired()
            .HasDefaultValue(AgeRestriction.None)
            .HasColumnName("age_restriction");
    }
}