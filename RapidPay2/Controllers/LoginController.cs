using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay2.DTOs;
using RapidPay2.Services;

namespace RapidPay2.Controllers;

[ApiController]
[Authorize("BasicAuthentication")]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LoginController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public IActionResult Login()
    {
        var token = authService.GenerateToken(User.Identity!.Name!);
        var authResponse = new AuthResponse
        {
            Token = token
        };
        return Ok(authResponse);
    }
}