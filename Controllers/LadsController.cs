
using farmaatte_api.DTOs;
using farmaatte_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace farmaatte_api.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
public class LadsController : V1ControllerBase
{

    private readonly ILogger<LadsController> _logger;
    private readonly FarmaatteDbContext _context;

    public LadsController(ILogger<LadsController> logger, FarmaatteDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetLad([FromHeader] int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        else
        {
            var lads = await _context.Users.Where(x => x.Groupid == user.Groupid).ToListAsync();
            return Ok(lads);
        }
    }

    [HttpGet("all/{userid}")]
    public async Task<IActionResult> getAllLads(int userid)
    {
        var user = await _context.Users.FindAsync(userid);
        var allDTOs = new List<ProfileOverviewDTO>();
        if (user == null)
        {
            return NotFound();
        }
        else
        {
            var allLads = await _context.Users.Where(x => x.Groupid == user.Groupid).ToListAsync();
            foreach (User lad in allLads)
            {
                var image = await _context.Profilepictures.Where(x => x.Userid == lad.Userid).Select(x => x.Image).FirstOrDefaultAsync();
                var dto = new ProfileOverviewDTO
                {
                    Id = lad.Userid,
                    Name = lad.Name,
                    Nickname = lad.Nickname,
                    Picture = image
                };
                allDTOs.Add(dto);
            }
            return Ok(allDTOs);
        }
    }
}