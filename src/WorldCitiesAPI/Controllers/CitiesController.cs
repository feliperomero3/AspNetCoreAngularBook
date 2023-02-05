using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;
using WorldCitiesAPI.Models;

namespace WorldCitiesAPI.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CitiesController(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<City>>> GetCities(
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
    {
        var result = await ApiResult<City>.CreateAsync(
            _context.Cities.AsNoTracking(), pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(long id)
    {
        var city = await _context.Cities.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        return city;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(long id, City city)
    {
        if (id != city.CityId)
        {
            return BadRequest();
        }

        _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
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
    public async Task<ActionResult<City>> PostCity(City city)
    {
        _context.Cities.Add(city);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCity", new { id = city.CityId }, city);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(long id)
    {
        var city = await _context.Cities.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        _context.Cities.Remove(city);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(long id)
    {
        return (_context.Cities?.Any(e => e.CityId == id)).GetValueOrDefault();
    }
}
