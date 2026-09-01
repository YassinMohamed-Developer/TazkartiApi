using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class TicketPassConfiguration : IEntityTypeConfiguration<TicketPass>
{
    public void Configure(EntityTypeBuilder<TicketPass> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.CurrentFanId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.OriginalFanId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.HolderName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(t => t.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Gate)
            .HasMaxLength(50);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(t => t.Competition)
            .HasMaxLength(150);

        builder.Property(t => t.Round)
            .HasMaxLength(50);

        builder.Property(t => t.Title)
            .HasMaxLength(200);

        builder.Property(t => t.HomeTeam)
            .HasMaxLength(150);

        builder.Property(t => t.AwayTeam)
            .HasMaxLength(150);

        builder.HasOne(t => t.BookingOrder)
            .WithMany(b => b.Tickets)
            .HasForeignKey(t => t.BookingOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
