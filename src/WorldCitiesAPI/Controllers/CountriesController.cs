using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;
using WorldCitiesAPI.Models;

namespace WorldCitiesAPI.Controllers;

[Authorize(Policy = "UserPolicy")]
[Route("api/countries")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CountriesController(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<CountryModel>>> GetCountries(
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
    {
        var countries = _context.Countries.AsNoTracking()
            .Select(c => new CountryModel
            {
                CountryId = c.CountryId,
                Name = c.Name,
                Iso2 = c.Iso2,
                Iso3 = c.Iso3,
                CitiesCount = c.Cities.Count()
            });

        var result = await ApiResult<CountryModel>.CreateAsync(
            countries, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(long id)
    {
        var country = await _context.Countries.FindAsync(id);

        if (country == null)
        {
            return NotFound();
        }

        return country;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(long id, CountryModel countryModel)
    {
        var country = _context.Countries.Find(id);

        if (country == null)
        {
            return new StatusCodeResult(StatusCodes.Status422UnprocessableEntity);
        }

        _context.Entry(country).CurrentValues.SetValues(countryModel);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CountryInputModel country)
    {
        var newCountry = new Country(country.Name, country.Iso2, country.Iso3);

        _context.Countries.Add(newCountry);

        await _context.SaveChangesAsync();

        var countryModel = new CountryModel
        {
            CountryId = newCountry.CountryId,
            Name = newCountry.Name,
            Iso2 = newCountry.Iso2,
            Iso3 = newCountry.Iso3
        };

        return CreatedAtAction(nameof(GetCountry), new { id = countryModel.CountryId }, countryModel);
    }

    [Authorize(Policy = "AdministratorPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(long id)
    {
        var country = await _context.Countries.FindAsync(id);

        if (country == null)
        {
            return NotFound();
        }

        _context.Countries.Remove(country);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CountryExists(long id)
    {
        return (_context.Countries?.Any(e => e.CountryId == id)).GetValueOrDefault();
    }
}
