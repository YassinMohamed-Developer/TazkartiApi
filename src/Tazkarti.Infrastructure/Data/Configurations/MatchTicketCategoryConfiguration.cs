using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class MatchTicketCategoryConfiguration : IEntityTypeConfiguration<MatchTicketCategory>
{
    public void Configure(EntityTypeBuilder<MatchTicketCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.Color)
            .HasMaxLength(30);

        builder.Property(c => c.GateAllocation)
            .HasMaxLength(100);

        builder.HasOne(c => c.Match)
            .WithMany(m => m.TicketCategories)
            .HasForeignKey(c => c.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
