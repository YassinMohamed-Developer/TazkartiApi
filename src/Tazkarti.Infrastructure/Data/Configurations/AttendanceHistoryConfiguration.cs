using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class AttendanceHistoryConfiguration : IEntityTypeConfiguration<AttendanceHistory>
{
    public void Configure(EntityTypeBuilder<AttendanceHistory> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(a => a.MatchTitle)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Venue)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(a => a.User)
            .WithMany(u => u.AttendanceHistories)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
