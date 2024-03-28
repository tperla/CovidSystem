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
    // POST endpoint to upload an image for a member
    [HttpPost("UploadImage")]
    public async Task<ActionResult<string>> UploadImage(string memberId, IFormFile file)
    {
        // Find the member by memberId
        var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == memberId);
        if (member != null)
            member.ImageHash = await _imageService.GetImageHash(file);// Set the image hash for the member
        else
            return NotFound();
        return Ok(member.ImageHash);
    }
    // POST endpoint to create a new member
    [HttpPost("CreateMember")]
    public async Task<ActionResult<Member>> CreateMember(Member member)
    {
        var validationResults = _ValidationService.ValidateMember(member);// Validate the member using IValidationService
        if (validationResults.Any())
        {
            return BadRequest(validationResults);// Return validation errors if any
        }
        try
        {

            _context.Members.Add(member);// Add the member to the context
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
    // GET endpoint to retrieve a member by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetMember(string id)
    {
        try
        {
            // Retrieve the member by ID including vaccinations and manufacturer
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
    // GET endpoint to retrieve all members
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
    {
        try
        {
            // Retrieve all members including vaccinations and manufacturer
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
