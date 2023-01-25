using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCitiesAPI.Entities;

namespace WorldCitiesAPI.Data.Configurations;

/// <summary>
/// EF Core configuration for <see cref="City"/>.
/// </summary>
public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities")
            .HasKey(t => t.CityId);

        builder.Property(t => t.CityId)
            .HasColumnType("bigint")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(t => t.Name)
            .HasColumnType("varchar")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(t => t.Latitude)
            .HasColumnType("decimal(7,4)")
            .IsRequired();

        builder.Property(t => t.Longitude)
            .HasColumnType("decimal(7,4)")
            .IsRequired();

        builder.HasIndex(b => b.Name);

        builder.HasIndex(new[] { "Name", "Latitude", "Longitude" })
            .IsUnique();
    }
}
