using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserActivityRepository
{
    Task<UserActivity?> GetByIdAsync(int id);
    Task<IEnumerable<UserActivity>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 20);
    Task<UserActivity> AddAsync(UserActivity activity);
    Task<UserActivity> UpdateAsync(UserActivity activity);
    Task DeleteAsync(int id);
}