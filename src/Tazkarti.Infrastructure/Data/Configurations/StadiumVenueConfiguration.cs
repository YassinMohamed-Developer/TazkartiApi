using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class StadiumVenueConfiguration : IEntityTypeConfiguration<StadiumVenue>
{
    public void Configure(EntityTypeBuilder<StadiumVenue> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(v => v.Location)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(v => v.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.MetroAccess)
            .HasMaxLength(150);

        builder.Property(v => v.ImageUrl)
            .HasMaxLength(500);

        builder.Property(v => v.Description)
            .HasMaxLength(1000);
    }
}
