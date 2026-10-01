using ShopActivityMonitoring.Application.DTOs;

namespace ShopActivityMonitoring.Application.Interfaces;

public interface IShopEventService
{
    Task<ShopEventDto> CreateEventAsync(CreateShopEventDto dto);
    Task<List<ShopEventDto>> GetEventsAsync(ShopEventFilterDto filter);
    Task<bool> DeleteEventAsync(int id);
    Task<int> GenerateTestEventsAsync(int count);
}