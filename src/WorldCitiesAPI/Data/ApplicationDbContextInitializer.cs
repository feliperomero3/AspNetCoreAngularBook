using System.Globalization;
using CsvHelper;
using WorldCitiesAPI.Entities;

namespace WorldCitiesAPI.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IHostEnvironment _environment;

    public ApplicationDbContextInitializer(ApplicationDbContext context, IHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    internal void Initialize()
    {
        _context.Database.EnsureCreated();

        if (_context.Cities.Any())
        {
            return; // Database has been seeded.
        }

        var path = Path.Combine(_environment.ContentRootPath, "Data/Source/worldcities_test.csv");

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var cities = new HashSet<City>();
        var countries = new HashSet<Country>();

        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var city = csv.GetField<string>("city_ascii")!;
            var latitude = csv.GetField<decimal>("lat");
            var longitude = csv.GetField<decimal>("lng");

            var name = csv.GetField<string>("country")!;
            var iso2 = csv.GetField<string>("iso2")!;
            var iso3 = csv.GetField<string>("iso3")!;

            var country = new Country(name, iso2, iso3);

            countries.Add(country);

            _context.Cities.Add(new City(city, latitude, longitude, countries.Single(c => c.Name == country.Name)));
        }

        _context.Cities.AddRange(cities);

        _context.SaveChanges();
    }
}
