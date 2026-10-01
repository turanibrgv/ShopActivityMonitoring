namespace ShopActivityMonitoring.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public ICollection<ShopEvent> ShopEvents { get; set; } = new List<ShopEvent>();
}