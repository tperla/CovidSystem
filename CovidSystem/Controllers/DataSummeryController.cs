using CovidSystem.DbContexts;
using CovidSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CovidSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataSummeryController: ControllerBase
    {
        private readonly CovidDbContext _context;
        private readonly ISummeryDataService _summeryDataService;
        public DataSummeryController(CovidDbContext context, ISummeryDataService summeryDataService)
        {
            _context = context;
            _summeryDataService = summeryDataService;
        }
        // Endpoint for retrieving graph data
        [HttpGet("GetGraphData")]
        public async Task<ActionResult<IEnumerable<object>>> GetGraphData()
        {
            try
            {
                var members = await _context.Members
                                           .Include(m => m.Vaccinations) // Include vaccinations
                                               .ThenInclude(v => v.Manufacturer) // Include manufacturer for each vaccination
                                           .ToListAsync();

                var summeryData = _summeryDataService.GetGraphData(members);

                return Ok(summeryData);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        // Endpoint for retrieving the count of not vaccinated members
        [HttpGet("GetNotVaccinatedMembers")]
        public async Task<ActionResult<int>> GetNotVaccinatedMembers()
        {
            try
            {
                var members = await _context.Members
                                           .Include(m => m.Vaccinations) 
                                               .ThenInclude(v => v.Manufacturer) 
                                           .ToListAsync();

                var notVaccinatedMembersCount = _summeryDataService.GetNotVaccinatedMembers(members);

                return Ok(notVaccinatedMembersCount); 
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
