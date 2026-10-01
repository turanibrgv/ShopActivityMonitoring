using ShopActivityMonitoring.Domain.Entities;

namespace ShopActivityMonitoring.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
}