using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AspNetUsers", t =>
        {
            t.HasCheckConstraint("CK_AppUser_NationalId", "LEN([Id]) = 14 AND [Id] NOT LIKE '%[^0-9]%'");
        });

        builder.Property(u => u.Id)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(u => u.FanId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(u => u.FanId)
            .IsUnique();

        builder.Property(u => u.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.Governorate)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(u => u.Nationality)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Gender)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(u => u.LoyaltyTier)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
