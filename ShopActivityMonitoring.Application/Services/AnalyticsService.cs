using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ShopActivityMonitoring.Application.Common;
using ShopActivityMonitoring.Application.DTOs;
using ShopActivityMonitoring.Application.Interfaces;
using ShopActivityMonitoring.Domain.Enums;

namespace ShopActivityMonitoring.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IShopEventRepository _eventRepository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(
        IShopEventRepository eventRepository,
        IMemoryCache cache,
        ILogger<AnalyticsService> logger)
    {
        _eventRepository = eventRepository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<AnalyticsSummaryDto> GetSummaryAsync()
    {
        if (_cache.TryGetValue(CacheKeys.AnalyticsSummary, out AnalyticsSummaryDto? cached) && cached != null)
        {
            _logger.LogInformation("Аналитика получена из кэша");
            return cached;
        }

        _logger.LogInformation("Расчёт аналитики (кэш пуст или истёк)");

        var events = await _eventRepository.GetAllAsync();

        var views = events.Count(e => e.EventType == EventType.ViewProduct);
        var addToCart = events.Count(e => e.EventType == EventType.AddToCart);
        var orders = events.Where(e => e.EventType == EventType.PlaceOrder).ToList();

        var summary = new AnalyticsSummaryDto
        {
            TotalEvents = events.Count,
            TotalViews = views,
            TotalAddToCart = addToCart,
            TotalOrders = orders.Count,
            ConversionRate = views == 0 ? 0 : Math.Round(orders.Count * 100.0 / views, 2),
            TotalRevenue = orders.Sum(e => e.TotalAmount),
            RevenueByCategory = orders
                .Where(e => e.Product != null)
                .GroupBy(e => e.Product!.Category)
                .Select(g => new CategoryStatDto
                {
                    Category = g.Key,
                    Revenue = g.Sum(e => e.TotalAmount),
                    OrdersCount = g.Count()
                })
                .OrderByDescending(c => c.Revenue)
                .ToList(),
            TopProducts = events
                .Where(e => e.Product != null)
                .GroupBy(e => e.Product!.Name)
                .Select(g => new ProductStatDto
                {
                    ProductName = g.Key,
                    ViewsCount = g.Count(e => e.EventType == EventType.ViewProduct),
                    OrdersCount = g.Count(e => e.EventType == EventType.PlaceOrder)
                })
                .OrderByDescending(p => p.OrdersCount)
                .Take(5)
                .ToList()
        };

        _cache.Set(CacheKeys.AnalyticsSummary, summary, TimeSpan.FromSeconds(60));

        return summary;
    }
}