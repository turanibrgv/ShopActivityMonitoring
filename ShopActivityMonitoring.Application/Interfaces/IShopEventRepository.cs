using ShopActivityMonitoring.Domain.Entities;

namespace ShopActivityMonitoring.Application.Interfaces;

public interface IShopEventRepository
{
    Task<ShopEvent> AddAsync(ShopEvent shopEvent);
    Task<ShopEvent?> GetByIdAsync(int id);
    Task<List<ShopEvent>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
}