using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;
using WorldCitiesAPI.Models;

namespace WorldCitiesAPI.Controllers;

[Authorize(Policy = "UserPolicy")]
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
    public async Task<ActionResult<ApiResult<CityModel>>> GetCities(
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
    {
        var cities = _context.Cities.AsNoTracking()
            .Select(c => new CityModel
            {
                CityId = c.CityId,
                Name = c.Name,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                CountryId = c.Country!.CountryId,
                CountryName = c.Country.Name
            });

        var result = await ApiResult<CityModel>.CreateAsync(
            cities, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityModel>> GetCity(long id)
    {
        var city = await _context.Cities.AsNoTracking()
            .Include(city => city.Country)
            .SingleAsync(city => city.CityId == id);

        if (city == null)
        {
            return NotFound();
        }

        var model = new CityModel
        {
            CityId = id,
            Name = city.Name,
            Latitude = city.Latitude,
            Longitude = city.Longitude,
            CountryId = city.Country!.CountryId
        };

        return model;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(long id, CityInputModel cityModel)
    {
        var city = await _context.Cities
            .Include(city => city.Country)
            .SingleAsync(city => city.CityId == id);

        if (city == null)
        {
            return new StatusCodeResult(StatusCodes.Status422UnprocessableEntity);
        }

        _context.Entry(city).CurrentValues.SetValues(cityModel);

        if (city.Country?.CountryId != cityModel.CountryId)
        {
            var country = await _context.Countries.FindAsync(cityModel.CountryId);

            if (country == null)
            {
                return new StatusCodeResult(StatusCodes.Status422UnprocessableEntity);
            }

            city.UpdateCountry(country);
        }

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
    public async Task<ActionResult<City>> PostCity(CityInputModel cityModel)
    {
        var country = await _context.Countries.FindAsync(cityModel.CountryId);

        if (country == null)
        {
            return new StatusCodeResult(StatusCodes.Status422UnprocessableEntity);
        }

        var newCity = new City(cityModel.Name, cityModel.Latitude, cityModel.Longitude, country);

        _context.Cities.Add(newCity);

        await _context.SaveChangesAsync();

        var model = new CityModel
        {
            CityId = newCity.CityId,
            Name = newCity.Name,
            Latitude = newCity.Latitude,
            Longitude = newCity.Longitude,
            CountryId = newCity.Country!.CountryId
        };

        return CreatedAtAction(nameof(GetCity), new { id = newCity.CityId }, model);
    }

    [Authorize(Policy = "AdministratorPolicy")]
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
