using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class BookingOrderConfiguration : IEntityTypeConfiguration<BookingOrder>
{
    public void Configure(EntityTypeBuilder<BookingOrder> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookingReference)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(b => b.BookingReference)
            .IsUnique();

        builder.Property(b => b.UserId)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(b => b.BookingType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(b => b.Venue)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(b => b.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.Gate)
            .HasMaxLength(50);

        builder.Property(b => b.Block)
            .HasMaxLength(50);

        builder.Property(b => b.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Match)
            .WithMany(m => m.Bookings)
            .HasForeignKey(b => b.MatchId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Event)
            .WithMany(e => e.Bookings)
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Tier)
            .WithMany()
            .HasForeignKey(b => b.TierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
