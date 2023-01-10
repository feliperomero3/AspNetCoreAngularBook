namespace WorldCitiesAPI.Entities;

/// <summary>
/// Represents a city.
/// </summary>
public class City
{
    /// <summary>
    /// The city unique Id.
    /// </summary>
    public long CityId { get; private set; }

    /// <summary>
    /// The city name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The city latitude.
    /// </summary>
    public decimal Latitude { get; private set; }

    /// <summary>
    /// The city longitude.
    /// </summary>
    public decimal Longitude { get; private set; }

    /// <summary>
    /// The country the city belongs to.
    /// </summary>
    public Country Country { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="City"/> class.
    /// </summary>
    /// <param name="id">The city unique Id.</param>
    /// <param name="name">The city name.</param>
    /// <param name="latitude">The city latitude.</param>
    /// <param name="longitude">The city longitude.</param>
    /// <param name="country">The country the city belongs to.</param>
    internal City(long id, string name, decimal latitude, decimal longitude, Country country)
    {
        CityId = id;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Country = country;
    }
}
