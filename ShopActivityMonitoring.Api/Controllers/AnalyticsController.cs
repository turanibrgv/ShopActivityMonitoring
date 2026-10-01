using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopActivityMonitoring.Application.Interfaces;

namespace ShopActivityMonitoring.Api.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize(Roles = "Analyst,Admin")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    // Сводная аналитика: конверсия, выручка по категориям, топ товаров
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _analyticsService.GetSummaryAsync();
        return Ok(summary);
    }
}