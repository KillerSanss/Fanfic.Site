using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности Comment для базы данных
/// </summary>
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasColumnName("user_id");
        
        builder.Property(p => p.ChapterId)
            .IsRequired()
            .HasColumnName("chapter_id");
        
        builder.Property(p => p.ParentCommentId)
            .HasColumnName("parent_comment_id");
        
        builder.Property(p => p.Content)
            .IsRequired()
            .HasColumnName("content");
        
        builder.HasOne(p => p.User)
            .WithMany(p => p.Comments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Chapter)
            .WithMany(p => p.Comments) 
            .HasForeignKey(p => p.ChapterId) 
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(c => c.ParentComment)
            .WithMany()
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}