using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Identity;
using WorldCitiesAPI.Entities;

namespace WorldCitiesAPI.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IHostEnvironment _environment;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitializer(
        ApplicationDbContext context,
        IHostEnvironment environment,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _environment = environment;
        _roleManager = roleManager;
        _userManager = userManager;
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

        if (!_roleManager.Roles.Any())
        {
            _roleManager.CreateAsync(new ApplicationRole("User")).Wait();
            _roleManager.CreateAsync(new ApplicationRole("Administrator")).Wait();
        }

        if (!_userManager.Users.Any())
        {
            var alice = new ApplicationUser("alice@example.com")
            {
                Id = "254188e3-c181-48f4-8004-91135a038037",
                Email = "alice@example.com",
                EmailConfirmed = true
            };
            _userManager.CreateAsync(alice, "password").Wait();
            _userManager.AddToRoleAsync(alice, "Administrator").Wait();

            var bob = new ApplicationUser("bob@example.com")
            {
                Id = "e498a65f-f467-4d75-a402-24ea663fa754",
                Email = "bob@example.com",
                EmailConfirmed = true
            };
            _userManager.CreateAsync(bob, "password").Wait();
            _userManager.AddToRoleAsync(bob, "User").Wait();

            var anna = new ApplicationUser("anna@example.com")
            {
                Id = "a7820f95-7692-42fd-a2a3-c97ed12bd89a",
                Email = "anna@example.com",
                EmailConfirmed = false
            };
            _userManager.CreateAsync(anna, "password").Wait();
        }
    }
}
