

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using farmaatte_api.DTOs;
using farmaatte_api.Models;
using farmaatte_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace farmaatte_api.Controllers;

[Route("api/v1/[controller]")]
public class LoginController : V1ControllerBase
{

    private readonly FarmaatteDbContext _context;
    private readonly ILogger<LoginController> _logger;

    private readonly IConfiguration _config;


    public LoginController(ILogger<LoginController> logger, FarmaatteDbContext context, IConfiguration config)
    {
        _context = context;
        _logger = logger;
        _config = config;
    }

    [HttpPost]
    [AllowAnonymous]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = await _context.Users.Where(x => x.Username == dto.Username).FirstOrDefaultAsync();
        if (user == null)
        {
            Thread.Sleep(3000);
            return Unauthorized();
        }
        else
        {
            var hashEntered = BCrypt.Net.BCrypt.HashPassword(dto.Password, user.Salt);
            if (hashEntered.Trim() != user.Pwhash.Trim())
            {
                Thread.Sleep(3000);
                return Unauthorized();
            }
            else
            {
                // TODO: issue token and return okay
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.Now.AddMinutes(120);
                var secToken = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: expiration,
                    signingCredentials: credentials);
                var token = new JwtSecurityTokenHandler().WriteToken(secToken);
                LoginSucceededDTO returnDto = new(token, ((DateTimeOffset)expiration).ToUnixTimeMilliseconds(), user.Userid);
                return Ok(returnDto);
            }
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("create")]
    public IActionResult createUser([FromBody] CreateUserDTO dto)
    {
        var checkForUsername = _context.Users.Where(x => x.Username == dto.Username).FirstOrDefault();
        if (checkForUsername != null)
        {
            return BadRequest();
        }
        else
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var newUser = new User
            {
                Username = dto.Username,
                Name = dto.Name,
                Nickname = dto.Nickname,
                Groupid = dto.Groupid,
                Pwhash = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt),
                Salt = salt
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok();
        }

    }
}