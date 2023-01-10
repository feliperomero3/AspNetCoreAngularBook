namespace WorldCitiesAPI.Entities;

/// <summary>
/// Represents a country.
/// </summary>
public class Country
{
    /// <summary>
    /// The country unique Id.
    /// </summary>
    public long CountryId { get; set; }

    /// <summary>
    /// The country name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The country code in ISO 3166-1 ALPHA-2 format.
    /// </summary>
    public string Iso2 { get; set; }

    /// <summary>
    /// The country code in ISO 3166-1 ALPHA-3 format.
    /// </summary>
    public string Iso3 { get; set; }

    /// <summary>
    /// The cities that constitute the country.
    /// </summary>
    public IReadOnlyCollection<City> Cities { get; set; } = new HashSet<City>();

    /// <summary>
    /// Initializes a new instance of <see cref="Country"/> class.
    /// </summary>
    /// <param name="countryId">The country unique Id.</param>
    /// <param name="name">The country name.</param>
    /// <param name="iso2">The country code in ISO 3166-1 ALPHA-2 format.</param>
    /// <param name="iso3">The country code in ISO 3166-1 ALPHA-3 format.</param>
    internal Country(long countryId, string name, string iso2, string iso3)
    {
        CountryId = countryId;
        Name = name;
        Iso2 = iso2;
        Iso3 = iso3;
    }
}
