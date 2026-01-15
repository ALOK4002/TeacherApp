using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(int userId);
    Task<UserProfile?> GetByIdAsync(int id);
    Task<UserProfile> AddAsync(UserProfile profile);
    Task<UserProfile> UpdateAsync(UserProfile profile);
    Task<bool> DeleteAsync(int id);
    Task<bool> HasProfileAsync(int userId);
}
