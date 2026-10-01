using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopActivityMonitoring.Application.DTOs;
using ShopActivityMonitoring.Application.Interfaces;

namespace ShopActivityMonitoring.Api.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IShopEventService _eventService;

    public EventsController(IShopEventService eventService)
    {
        _eventService = eventService;
    }

    // Создание события (просмотр товара / добавление в корзину / заказ)
    [HttpPost]
    [Authorize(Roles = "Operator,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateShopEventDto dto)
    {
        var result = await _eventService.CreateEventAsync(dto);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    // Получение списка событий с фильтрами (EventType, Category, даты, страницы)
    [HttpGet]
    [Authorize(Roles = "Operator,Analyst,Admin")]
    public async Task<IActionResult> GetAll([FromQuery] ShopEventFilterDto filter)
    {
        var result = await _eventService.GetEventsAsync(filter);
        return Ok(result);
    }

    // Удаление события — только Admin
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _eventService.DeleteEventAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}