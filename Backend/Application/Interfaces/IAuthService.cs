using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<PendingUsersResponse> GetPendingUsersAsync();
    Task<List<UserDto>> GetAllUsersAsync();
    Task ApproveUserAsync(int userId, int approvedByUserId);
    Task RejectUserAsync(int userId, string rejectionReason, int rejectedByUserId);
}