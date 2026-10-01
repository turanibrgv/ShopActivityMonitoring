using Microsoft.EntityFrameworkCore;
using ShopActivityMonitoring.Application.Interfaces;
using ShopActivityMonitoring.Domain.Entities;
using ShopActivityMonitoring.Infrastructure.Data;

namespace ShopActivityMonitoring.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.Include(u => u.Role).AsNoTracking().ToListAsync();
    }
}