using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUserNameOrEmailAsync(string value);
    Task AddAsync(User user);
}