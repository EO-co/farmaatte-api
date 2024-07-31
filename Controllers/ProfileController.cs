
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
public class ProfileController : V1ControllerBase
{

    private readonly ILogger<ProfileController> _logger;
    private readonly FarmaatteDbContext _context;

    public ProfileController(ILogger<ProfileController> logger, FarmaatteDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var profile = await _context.Users.FindAsync(id);
        if (profile == null)
        {
            return NotFound();
        }
        else
        {
            var dto = new ProfileDTO
            {
                Name = profile.Name,
                Nickname = profile.Nickname,
                ProfilePicture = await _context.Profilepictures.Where(x => x.Userid == id).Select(x => x.Image).FirstOrDefaultAsync()
            };
            return Ok(profile);
        }
    }

    [HttpPost("picture/{id}")]
    public async Task<IActionResult> SaveProfilePicture([FromRoute] int id, [FromForm] IFormFile picture)
    {
        if (picture.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                picture.CopyTo(ms);
                var filebytes = ms.ToArray();

                var NewPicture = new Profilepicture
                {
                    Userid = id,
                    Image = filebytes
                };
                try
                {
                    await _context.Profilepictures.AddAsync(NewPicture);
                    _context.SaveChanges();
                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
        }
        else
        {
            return BadRequest();
        }
    }

}