using ShopActivityMonitoring.Domain.Enums;

namespace ShopActivityMonitoring.Domain.Entities;

public class ShopEvent
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public EventType EventType { get; set; }

    // Новое поле модели — обязательное требование адаптации задания
    public int Quantity { get; set; } = 1;

    // Сумма события (заполняется, когда EventType = PlaceOrder)
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Source { get; set; } = "web"; // web, mobile, api
}