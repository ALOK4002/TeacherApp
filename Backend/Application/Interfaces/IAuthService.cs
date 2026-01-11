using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequestDto dto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
}