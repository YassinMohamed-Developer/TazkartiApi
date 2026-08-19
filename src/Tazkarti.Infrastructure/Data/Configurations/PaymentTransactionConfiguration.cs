using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.CardNumberMasked)
            .HasMaxLength(30);

        builder.Property(p => p.FawryCode)
            .HasMaxLength(50);

        builder.Property(p => p.MobileWalletNumber)
            .HasMaxLength(30);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.HasOne(p => p.BookingOrder)
            .WithMany(b => b.Transactions)
            .HasForeignKey(p => p.BookingOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.User)
            .WithMany(u => u.PaymentTransactions)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
