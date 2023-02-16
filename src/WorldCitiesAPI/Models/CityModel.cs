namespace WorldCitiesAPI.Models;

/// <summary>
/// A City
/// </summary>
public class CityModel
{
    /// <summary>
    /// The city unique Id.
    /// </summary>
    public long CityId { get; set; }

    /// <summary>
    /// The city name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The city latitude.
    /// </summary>
    public decimal Latitude { get; set; }

    /// <summary>
    /// The city longitude.
    /// </summary>
    public decimal Longitude { get; set; }

    /// <summary>
    /// The country unique Id the city belongs to.
    /// </summary>
    public long CountryId { get; set; }

    /// <summary>
    /// The country unique Id the city belongs to.
    /// </summary>
    public string CountryName { get; set; } = string.Empty;
}
