using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCitiesAPI.Entities;

namespace WorldCitiesAPI.Data.Configurations;

/// <summary>
/// EF Core configuration for <see cref="Country"/>.
/// </summary>
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries").HasKey(t => t.CountryId);

        builder.Property(t => t.CountryId)
            .HasColumnType("bigint")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(t => t.Name)
            .HasColumnType("varchar")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(t => t.Iso2)
            .HasColumnType("varchar")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(t => t.Iso3)
            .HasColumnType("varchar")
            .HasMaxLength(3)
            .IsRequired();

        builder.HasMany(t => t.Cities)
            .WithOne(t => t.Country)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.Name)
            .IsUnique();
        builder.HasIndex(b => b.Iso2)
            .IsUnique();
        builder.HasIndex(b => b.Iso3)
            .IsUnique();
    }
}
