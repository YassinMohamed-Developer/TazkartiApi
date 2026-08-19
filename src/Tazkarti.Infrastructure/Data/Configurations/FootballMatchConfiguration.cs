using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class FootballMatchConfiguration : IEntityTypeConfiguration<FootballMatch>
{
    public void Configure(EntityTypeBuilder<FootballMatch> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Competition)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(m => m.Round)
            .HasMaxLength(50);

        builder.Property(m => m.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.KickoffTime)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.GateOpenTime)
            .HasMaxLength(20);

        builder.Property(m => m.AvailabilityStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(m => m.MinPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(m => m.BannerImage)
            .HasMaxLength(500);

        builder.HasOne(m => m.HomeTeam)
            .WithMany(c => c.HomeMatches)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.AwayTeam)
            .WithMany(c => c.AwayMatches)
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Venue)
            .WithMany(v => v.Matches)
            .HasForeignKey(m => m.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
