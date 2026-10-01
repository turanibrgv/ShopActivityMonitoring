using ShopActivityMonitoring.Domain.Enums;

namespace ShopActivityMonitoring.Application.DTOs;

public class ShopEventDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public EventType EventType { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Source { get; set; } = string.Empty;
}

public class CreateShopEventDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public EventType EventType { get; set; }
    public int Quantity { get; set; } = 1;
    public string Source { get; set; } = "web";
}

// Фильтр API: EventType + новый фильтр по Category (требование адаптации)
public class ShopEventFilterDto
{
    public EventType? EventType { get; set; }
    public string? Category { get; set; }
    public int? UserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}