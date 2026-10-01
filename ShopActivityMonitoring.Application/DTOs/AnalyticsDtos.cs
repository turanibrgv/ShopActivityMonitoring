namespace ShopActivityMonitoring.Application.DTOs;

public class AnalyticsSummaryDto
{
    public int TotalEvents { get; set; }
    public int TotalViews { get; set; }
    public int TotalAddToCart { get; set; }
    public int TotalOrders { get; set; }
    public double ConversionRate { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<CategoryStatDto> RevenueByCategory { get; set; } = new();
    public List<ProductStatDto> TopProducts { get; set; } = new();
}

public class CategoryStatDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int OrdersCount { get; set; }
}

public class ProductStatDto
{
    public string ProductName { get; set; } = string.Empty;
    public int ViewsCount { get; set; }
    public int OrdersCount { get; set; }
}