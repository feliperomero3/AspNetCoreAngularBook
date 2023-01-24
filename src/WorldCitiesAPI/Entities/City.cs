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
    public Country? Country { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="City"/> class.
    /// </summary>
    /// <param name="cityId">The city unique Id.</param>
    /// <param name="name">The city name.</param>
    /// <param name="latitude">The city latitude.</param>
    /// <param name="longitude">The city longitude.</param>
    internal City(long cityId, string name, decimal latitude, decimal longitude)
    {
        CityId = cityId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="City"/> class.
    /// </summary>
    /// <param name="name">The city name.</param>
    /// <param name="latitude">The city latitude.</param>
    /// <param name="longitude">The city longitude.</param>
    internal City(string name, decimal latitude, decimal longitude) : this(0L, name, latitude, longitude)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not City other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Name).GetHashCode();
    }
}