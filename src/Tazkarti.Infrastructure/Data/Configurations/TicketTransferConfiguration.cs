using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class TicketTransferConfiguration : IEntityTypeConfiguration<TicketTransfer>
{
    public void Configure(EntityTypeBuilder<TicketTransfer> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.SenderFanId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.RecipientFanId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.HasOne(t => t.TicketPass)
            .WithMany(p => p.Transfers)
            .HasForeignKey(t => t.TicketPassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
