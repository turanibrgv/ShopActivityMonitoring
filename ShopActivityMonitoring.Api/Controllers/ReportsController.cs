using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopActivityMonitoring.Application.Interfaces;

namespace ShopActivityMonitoring.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Analyst,Admin")]
public class ReportsController : ControllerBase
{
    private readonly ICsvReportService _csvReportService;

    public ReportsController(ICsvReportService csvReportService)
    {
        _csvReportService = csvReportService;
    }

    // Выгрузка CSV-отчёта по всем событиям
    [HttpGet("events.csv")]
    public async Task<IActionResult> GetEventsCsv()
    {
        var bytes = await _csvReportService.GenerateEventsCsvAsync();
        return File(bytes, "text/csv", "events.csv");
    }
}