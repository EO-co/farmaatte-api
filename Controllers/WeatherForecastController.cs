using farmaatte_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace farmaatte_api.Controllers;

[Route("api/v1/weatherforecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly FarmaatteDbContext _context;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, FarmaatteDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Produces("application/json")]
    public IActionResult Get()
    {
        return Ok(_context.Groups.Find(1));
    }
}
