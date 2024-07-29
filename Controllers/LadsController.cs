
using farmaatte_api.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace farmaatte_api.Controllers;

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

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetLads([FromHeader] int userid)
    {
        var user = await _context.Users.FindAsync(userid);
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
}