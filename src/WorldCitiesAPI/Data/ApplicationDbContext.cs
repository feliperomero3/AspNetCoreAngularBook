using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Entities;

namespace WorldCitiesAPI.Data;

/// <summary>
/// Represents a session with the database and can be used to query and save instances of your entities.
/// </summary>
public class ApplicationDbContext : DbContext
{
    internal ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    internal DbSet<Country> Countries { get; set; }

    internal DbSet<City> City { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<City>().ToTable("Cities");
    }
}
