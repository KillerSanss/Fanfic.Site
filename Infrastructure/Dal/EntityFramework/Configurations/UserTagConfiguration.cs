using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

public class UserTagConfiguration : IEntityTypeConfiguration<UserTag>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<UserTag> builder)
    {
        builder.HasKey(p => new { p.UserId, p.TagId });
        
        builder.Property(p => p.UserId)
            .HasColumnName("user_id");
        
        builder.Property(p => p.TagId)
            .HasColumnName("tag_id");

        builder.HasOne<Tag>(p => p.Tag)
            .WithMany(p => p.UserTags)
            .HasForeignKey(p => p.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>(p => p.User)
            .WithMany(p => p.UserTags)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}