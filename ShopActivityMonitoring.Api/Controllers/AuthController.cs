using Microsoft.AspNetCore.Mvc;
using ShopActivityMonitoring.Application.DTOs;
using ShopActivityMonitoring.Application.Interfaces;

namespace ShopActivityMonitoring.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        return Ok(result);
    }
}