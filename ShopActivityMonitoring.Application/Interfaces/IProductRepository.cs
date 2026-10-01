using ShopActivityMonitoring.Domain.Entities;

namespace ShopActivityMonitoring.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
}