using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUserNameOrEmailAsync(string value);
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<List<User>> GetPendingUsersAsync();
    Task<List<User>> GetAllUsersAsync();
}