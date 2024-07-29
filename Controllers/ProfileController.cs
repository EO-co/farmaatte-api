
using farmaatte_api.Models;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace farmaatte_api.Controllers;

[Route("api/v1/[controller]")]
public class ProfileController : V1ControllerBase
{

    private readonly ILogger<ProfileController> _logger;
    private readonly FarmaatteDbContext _context;

    public ProfileController(ILogger<ProfileController> logger, FarmaatteDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    public IActionResult GetProfileData()
    {

        return Ok();
    }
}