using ShopActivityMonitoring.Application.DTOs;

namespace ShopActivityMonitoring.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
}