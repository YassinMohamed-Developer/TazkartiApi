using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TokenHash)
            .HasMaxLength(64)
            .IsUnicode(false)
            .IsFixedLength()
            .IsRequired();

        builder.Property(t => t.RowVersion)
            .IsRowVersion();

        builder.HasIndex(t => t.TokenHash)
            .IsUnique();

        builder.HasIndex(t => new { t.SessionId, t.ExpiresAtUtc });

        builder.HasOne(t => t.Session)
            .WithMany(s => s.RefreshTokens)
            .HasForeignKey(t => t.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.ReplacedByToken)
            .WithMany()
            .HasForeignKey(t => t.ReplacedByTokenId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
