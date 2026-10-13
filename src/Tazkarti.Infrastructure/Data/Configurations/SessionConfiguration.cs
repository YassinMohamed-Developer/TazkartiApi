using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.UserId)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(s => s.RefreshTokenHash)
            .HasMaxLength(64)
            .IsUnicode(false)
            .IsFixedLength()
            .IsRequired();

        builder.Property(s => s.RowVersion)
            .IsRowVersion();

        builder.HasIndex(s => s.RefreshTokenHash)
            .IsUnique();

        builder.HasIndex(s => new { s.UserId, s.ExpiresAtUtc });

        builder.HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
