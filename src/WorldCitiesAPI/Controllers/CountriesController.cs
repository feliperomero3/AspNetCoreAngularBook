using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;
using WorldCitiesAPI.Models;

namespace WorldCitiesAPI.Controllers;

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
    public async Task<ActionResult<ApiResult<Country>>> GetCountries(
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
    {
        var result = await ApiResult<Country>.CreateAsync(
            _context.Countries.AsNoTracking(), pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);

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
    public async Task<IActionResult> PutCountry(long id, Country country)
    {
        if (id != country.CountryId)
        {
            return BadRequest();
        }

        _context.Entry(country).State = EntityState.Modified;

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
    public async Task<ActionResult<Country>> PostCountry(Country country)
    {
        _context.Countries.Add(country);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCountry", new { id = country.CountryId }, country);
    }

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
