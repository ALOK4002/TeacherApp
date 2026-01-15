using Application.DTOs;

namespace Application.Interfaces;

public interface IUserProfileService
{
    Task<UserProfileDto?> GetMyProfileAsync(int userId);
    Task<UserProfileDto?> GetProfileByIdAsync(int id);
    Task<UserProfileDto> CreateProfileAsync(int userId, CreateUserProfileDto dto);
    Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateUserProfileDto dto);
    Task<bool> HasProfileAsync(int userId);
}
