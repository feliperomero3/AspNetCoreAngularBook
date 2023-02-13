namespace WorldCitiesAPI.Entities;

/// <summary>
/// Represents a country.
/// </summary>
public class Country
{
    /// <summary>
    /// The country unique Id.
    /// </summary>
    public long CountryId { get; private set; }

    /// <summary>
    /// The country name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The country code in ISO 3166-1 ALPHA-2 format.
    /// </summary>
    public string Iso2 { get; private set; }

    /// <summary>
    /// The country code in ISO 3166-1 ALPHA-3 format.
    /// </summary>
    public string Iso3 { get; private set; }

    /// <summary>
    /// The cities that constitute the country.
    /// </summary>
    public IReadOnlyCollection<City> Cities { get; private set; } = new HashSet<City>();

    /// <summary>
    /// Initializes a new instance of <see cref="Country"/> class.
    /// </summary>
    /// <param name="countryId">The country unique Id.</param>
    /// <param name="name">The country name.</param>
    /// <param name="iso2">The country code in ISO 3166-1 ALPHA-2 format.</param>
    /// <param name="iso3">The country code in ISO 3166-1 ALPHA-3 format.</param>
    internal Country(long countryId, string name, string iso2, string iso3)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(iso2))
        {
            throw new ArgumentException($"'{nameof(iso2)}' cannot be null or empty.", nameof(iso2));
        }

        if (string.IsNullOrEmpty(iso3))
        {
            throw new ArgumentException($"'{nameof(iso3)}' cannot be null or empty.", nameof(iso3));
        }

        CountryId = countryId;
        Name = name;
        Iso2 = iso2;
        Iso3 = iso3;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Country"/> class.
    /// </summary>
    /// <param name="name">The country name.</param>
    /// <param name="iso2">The country code in ISO 3166-1 ALPHA-2 format.</param>
    /// <param name="iso3">The country code in ISO 3166-1 ALPHA-3 format.</param>
    internal Country(string name, string iso2, string iso3) : this(0L, name, iso2, iso3)
    {
        Name = name;
        Iso2 = iso2;
        Iso3 = iso3;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Country other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Name).GetHashCode();
    }
}
