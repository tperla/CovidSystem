using CovidSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class VaccinationsController : ControllerBase
{
    private readonly CovidDbContext _context;
    private readonly IValidationService _ValidationService;
    public VaccinationsController(CovidDbContext context, IValidationService validationService)
    {
        _context = context;
        _ValidationService = validationService;
    }
    [HttpPost]
    public async Task<ActionResult<Vaccination>> CreateVaccination(Vaccination vaccination)
    {
        var validationResults = _ValidationService.ValidateVaccination(vaccination);
        if (validationResults.Any())
        {
            return BadRequest(validationResults);
        }
        try
        {
            // Check if the member already has 4 vaccinations
            var memberVaccinationCount = await _context.Vaccinations.CountAsync(v => v.MemberId == vaccination.MemberId);
            if (memberVaccinationCount >= 4)
            {
                return BadRequest("A member cannot have more than 4 vaccinations.");
            }
            _context.Vaccinations.Add(vaccination);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVaccination), new { id = vaccination.VaccinationId }, vaccination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Vaccination>> GetVaccination(int id)
    {
        try
        {
            var vaccination = await _context.Vaccinations.FindAsync(id);

            if (vaccination == null)
            {
                return NotFound(); 
            }
            return Ok(vaccination); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vaccination>>> GetVaccinations()
    {
        try
        {
            var vaccinations = await _context.Vaccinations.ToListAsync();

            return Ok(vaccinations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}

