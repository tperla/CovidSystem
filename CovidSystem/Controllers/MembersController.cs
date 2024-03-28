using CovidSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class MembersController : ControllerBase
{
    private readonly CovidDbContext _context;
    private readonly IValidationService _ValidationService;
    private readonly IImageService _imageService;
    // Constructor injecting CovidDbContext, IValidationService, and IImageService
    public MembersController(CovidDbContext context,
        IValidationService validationService,
        IImageService imageService)
    {
        _context = context;
        _ValidationService = validationService;
        _imageService = imageService;
    }

    [HttpPost("UploadImage")]
    public async Task<ActionResult<string>> UploadImage(string memberId, IFormFile file)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == memberId);
        if (member != null)
            member.ImageHash = await _imageService.GetImageHash(file);
        else
            return NotFound();
        return Ok(member.ImageHash);
    }

    [HttpPost("CreateMember")]
    public async Task<ActionResult<Member>> CreateMember(Member member)
    {
        var validationResults = _ValidationService.ValidateMember(member);
        if (validationResults.Any())
        {
            return BadRequest(validationResults);
        }
        try
        {

            _context.Members.Add(member);
            // If the member has vaccinations, add them to the context
            if (member.Vaccinations != null && member.Vaccinations.Any())
            {
                foreach (var vaccination in member.Vaccinations)
                {
                    _context.Vaccinations.Add(vaccination);
                }
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMember), new { id = member.MemberId }, member);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetMember(string id)
    {
        try
        {
            var member = await _context.Members
                               .Include(m => m.Vaccinations) // Include vaccinations
                                   .ThenInclude(v => v.Manufacturer) // Include manufacturer for each vaccination
                               .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
    {
        try
        {
            var members = await _context.Members
                               .Include(m => m.Vaccinations)
                                   .ThenInclude(v => v.Manufacturer)
                               .ToListAsync();

            return Ok(members);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
