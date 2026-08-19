using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class VenueGateConfiguration : IEntityTypeConfiguration<VenueGate>
{
    public void Configure(EntityTypeBuilder<VenueGate> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.GateName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(g => g.AllocatedFor)
            .HasMaxLength(150);

        builder.HasOne(g => g.Venue)
            .WithMany(v => v.Gates)
            .HasForeignKey(g => g.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
