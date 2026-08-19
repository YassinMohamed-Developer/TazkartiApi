using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class EntertainmentEventConfiguration : IEntityTypeConfiguration<EntertainmentEvent>
{
    public void Configure(EntityTypeBuilder<EntertainmentEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Category)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.Tag)
            .HasMaxLength(50);

        builder.Property(e => e.Artist)
            .HasMaxLength(150);

        builder.Property(e => e.EventTime)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.MinPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.BannerImage)
            .HasMaxLength(500);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
