

using farmaatte_api.DTOs;
using farmaatte_api.Models;
using farmaatte_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace farmaatte_api.Controllers;

[Route("api/v1/[controller]")]
public class LoginController : V1ControllerBase
{

    private readonly FarmaatteDbContext _context;
    private readonly ILogger<LoginController> _logger;

    private readonly HashingService _hashingService;

    public LoginController(ILogger<LoginController> logger, FarmaatteDbContext context, HashingService hashingService)
    {
        _context = context;
        _logger = logger;
        _hashingService = hashingService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = await _context.Users.FindAsync(dto.Username);
        if (user == null)
        {
            Thread.Sleep(3000);
            return Unauthorized();
        }
        else
        {
            var hash = _hashingService.HashString(user.Pwhash);
            if (dto.Password != hash)
            {
                Thread.Sleep(3000);
                return Unauthorized();
            }
            else
            {
                // TODO: issue token and return okay
                return Ok(user.Userid);
            }
        }
    }
}