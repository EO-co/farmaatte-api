

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace farmaatte_api.Controllers;

[Route("api/v1/[controller]")]
public class LoginController : V1ControllerBase
{

    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [Produces("application/json")]
    public IActionResult Login()
    {
        return Ok();
    }
}