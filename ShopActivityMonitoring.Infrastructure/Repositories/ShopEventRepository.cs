using Microsoft.EntityFrameworkCore;
using ShopActivityMonitoring.Application.Interfaces;
using ShopActivityMonitoring.Domain.Entities;
using ShopActivityMonitoring.Infrastructure.Data;

namespace ShopActivityMonitoring.Infrastructure.Repositories;

public class ShopEventRepository : IShopEventRepository
{
    private readonly AppDbContext _context;

    public ShopEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ShopEvent> AddAsync(ShopEvent shopEvent)
    {
        _context.ShopEvents.Add(shopEvent);
        await _context.SaveChangesAsync();
        return shopEvent;
    }

    public async Task<ShopEvent?> GetByIdAsync(int id)
    {
        return await _context.ShopEvents
            .Include(e => e.Product)
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<ShopEvent>> GetAllAsync()
    {
        return await _context.ShopEvents
            .Include(e => e.Product)
            .Include(e => e.User)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ShopEvents.FindAsync(id);
        if (entity == null) return false;

        _context.ShopEvents.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}