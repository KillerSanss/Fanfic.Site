using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Dal.EntityFramework.Configurations;

/// <summary>
/// Конфигурация сущности User для базы данных
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Метод конфигурации
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.NickName)
            .IsRequired()
            .HasColumnName("nickname");

        builder.Property(p => p.Email)
            .IsRequired()
            .HasColumnName("email");
        
        builder.Property(p => p.Password)
            .IsRequired()
            .HasColumnName("password");
        
        builder.Property(p => p.AvatarUrl)
            .IsRequired()
            .HasColumnName("avatar_url");
                
        builder.Property(p => p.RegistrationDate)
            .IsRequired()
            .HasColumnName("registration_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone");
        
        builder.Property(p => p.BirthDate)
            .IsRequired()
            .HasColumnName("birth_date")
            .HasColumnType("timestamp without time zone");
        
        builder.Property(p => p.TelegramId)
            .HasColumnName("telegram_id");

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasDefaultValue(Gender.None)
            .HasColumnName("gender");
        
        builder.Property(p => p.Role)
            .IsRequired()
            .HasDefaultValue(Role.None)
            .HasColumnName("role");
        
        builder.Property(p => p.IsEmail)
            .IsRequired()
            .HasColumnName("is_email");
        
        builder.Property(p => p.IsShowEmail)
            .IsRequired()
            .HasColumnName("is_show_email");
        
        builder.Property(p => p.IsTelegram)
            .IsRequired()
            .HasColumnName("is_telegram");
        
        builder.Property(p => p.IsShowTelegram)
            .IsRequired()
            .HasColumnName("is_show_telegram");
        
        builder.HasMany(p => p.Works)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Comments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}