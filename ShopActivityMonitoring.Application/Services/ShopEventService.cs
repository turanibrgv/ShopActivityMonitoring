using Microsoft.Extensions.Caching.Memory;
using ShopActivityMonitoring.Application.Common;
using ShopActivityMonitoring.Application.DTOs;
using ShopActivityMonitoring.Application.Interfaces;
using ShopActivityMonitoring.Domain.Entities;
using ShopActivityMonitoring.Domain.Enums;

namespace ShopActivityMonitoring.Application.Services;

public class ShopEventService : IShopEventService
{
    private readonly IShopEventRepository _eventRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;

    public ShopEventService(
        IShopEventRepository eventRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        IMemoryCache cache)
    {
        _eventRepository = eventRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _cache = cache;
    }

    public async Task<ShopEventDto> CreateEventAsync(CreateShopEventDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId)
            ?? throw new InvalidOperationException("Товар не найден");

        var shopEvent = new ShopEvent
        {
            UserId = dto.UserId,
            ProductId = dto.ProductId,
            EventType = dto.EventType,
            Quantity = dto.Quantity <= 0 ? 1 : dto.Quantity,
            Source = dto.Source,
            CreatedAt = DateTime.UtcNow
        };

        shopEvent.TotalAmount = dto.EventType == EventType.PlaceOrder
            ? product.Price * shopEvent.Quantity
            : 0;

        var saved = await _eventRepository.AddAsync(shopEvent);
        var user = await _userRepository.GetByIdAsync(dto.UserId);

        // сбрасываем кэш аналитики, т.к. появились новые данные
        _cache.Remove(CacheKeys.AnalyticsSummary);

        return MapToDto(saved, product, user);
    }

    public async Task<List<ShopEventDto>> GetEventsAsync(ShopEventFilterDto filter)
    {
        var events = await _eventRepository.GetAllAsync();
        var query = events.AsEnumerable();

        if (filter.EventType.HasValue)
            query = query.Where(e => e.EventType == filter.EventType.Value);

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(e => e.Product != null &&
                e.Product.Category.Equals(filter.Category, StringComparison.OrdinalIgnoreCase));

        if (filter.UserId.HasValue)
            query = query.Where(e => e.UserId == filter.UserId.Value);

        if (filter.FromDate.HasValue)
            query = query.Where(e => e.CreatedAt >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(e => e.CreatedAt <= filter.ToDate.Value);

        var paged = query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        return paged.Select(e => MapToDto(e, e.Product, e.User)).ToList();
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var deleted = await _eventRepository.DeleteAsync(id);
        if (deleted) _cache.Remove(CacheKeys.AnalyticsSummary);
        return deleted;
    }

    public async Task<int> GenerateTestEventsAsync(int count)
    {
        var products = await _productRepository.GetAllAsync();
        var users = await _userRepository.GetAllAsync();

        if (products.Count == 0 || users.Count == 0)
            return 0;

        var random = new Random();
        var eventTypes = Enum.GetValues<EventType>();
        var created = 0;

        for (int i = 0; i < count; i++)
        {
            var product = products[random.Next(products.Count)];
            var user = users[random.Next(users.Count)];
            var eventType = eventTypes[random.Next(eventTypes.Length)];
            var quantity = random.Next(1, 5);

            var shopEvent = new ShopEvent
            {
                UserId = user.Id,
                ProductId = product.Id,
                EventType = eventType,
                Quantity = quantity,
                Source = random.Next(2) == 0 ? "web" : "mobile",
                CreatedAt = DateTime.UtcNow.AddMinutes(-random.Next(0, 60 * 24 * 14)),
                TotalAmount = eventType == EventType.PlaceOrder ? product.Price * quantity : 0
            };

            await _eventRepository.AddAsync(shopEvent);
            created++;
        }

        _cache.Remove(CacheKeys.AnalyticsSummary);
        return created;
    }

    private static ShopEventDto MapToDto(ShopEvent e, Product? product, User? user)
    {
        return new ShopEventDto
        {
            Id = e.Id,
            UserId = e.UserId,
            UserName = user?.UserName ?? string.Empty,
            ProductId = e.ProductId,
            ProductName = product?.Name ?? string.Empty,
            Category = product?.Category ?? string.Empty,
            EventType = e.EventType,
            Quantity = e.Quantity,
            TotalAmount = e.TotalAmount,
            CreatedAt = e.CreatedAt,
            Source = e.Source
        };
    }
}