using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data.Configurations;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.ShortName)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(500);

        builder.Property(c => c.PrimaryColor)
            .HasMaxLength(30);
    }
}
