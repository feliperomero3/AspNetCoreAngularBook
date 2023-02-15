namespace WorldCitiesAPI.Models;

/// <summary>
/// A Country
/// </summary>
public class CountryModel
{
    /// <summary>
    /// The country unique Id.
    /// </summary>
    public long CountryId { get; set; }

    /// <summary>
    /// The country name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The country code in ISO 3166-1 ALPHA-2 format.
    /// </summary>
    public string Iso2 { get; set; } = string.Empty;

    /// <summary>
    /// The country code in ISO 3166-1 ALPHA-3 format.
    /// </summary>
    public string Iso3 { get; set; } = string.Empty;

    /// <summary>
    /// The total number of cities in the country.
    /// </summary>
    public int CitiesCount { get; set; }
}
