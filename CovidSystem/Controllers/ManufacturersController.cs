using CovidSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ManufacturersController : ControllerBase
{
    private readonly CovidDbContext _context;
    public ManufacturersController(CovidDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<ActionResult<Manufacturer>> CreateManufacturer(Manufacturer manufacturer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();// Save changes to the database

            return CreatedAtAction(nameof(GetManufacturer), new { id = manufacturer.ManufacturerId }, manufacturer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Manufacturer>> GetManufacturer(int id)
    {
        try
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);

            if (manufacturer == null)
            {
                return NotFound(); 
            }

            return Ok(manufacturer); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manufacturer>>> GetManufacturers()
    {
        try
        {
            var manufacturers = await _context.Manufacturers.ToListAsync();

            return Ok(manufacturers); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}"); 
        }
    }
}

