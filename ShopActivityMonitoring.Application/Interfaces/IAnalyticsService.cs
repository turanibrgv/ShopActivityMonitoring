using ShopActivityMonitoring.Application.DTOs;

namespace ShopActivityMonitoring.Application.Interfaces;

public interface IAnalyticsService
{
    Task<AnalyticsSummaryDto> GetSummaryAsync();
}