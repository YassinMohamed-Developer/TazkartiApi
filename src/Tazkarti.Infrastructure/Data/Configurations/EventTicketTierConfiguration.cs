using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class EventTicketTierConfiguration : IEntityTypeConfiguration<EventTicketTier>
{
    public void Configure(EntityTypeBuilder<EventTicketTier> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Perks)
            .HasMaxLength(2000);

        builder.HasOne(t => t.Event)
            .WithMany(e => e.TicketTiers)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
