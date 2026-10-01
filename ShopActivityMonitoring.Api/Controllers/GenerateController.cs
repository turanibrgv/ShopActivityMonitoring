using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopActivityMonitoring.Application.Interfaces;

namespace ShopActivityMonitoring.Api.Controllers;

[ApiController]
[Route("api/generate")]
[Authorize(Roles = "Admin")]
public class GenerateController : ControllerBase
{
    private readonly IShopEventService _eventService;

    public GenerateController(IShopEventService eventService)
    {
        _eventService = eventService;
    }

    // Генерация случайных тестовых событий для наполнения базы (нагрузочные данные)
    [HttpPost("events")]
    public async Task<IActionResult> GenerateEvents([FromQuery] int count = 500)
    {
        var created = await _eventService.GenerateTestEventsAsync(count);
        return Ok(new { created });
    }
}