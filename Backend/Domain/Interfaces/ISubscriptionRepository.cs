using Domain.Entities;

namespace Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByUserIdAsync(int userId);
    Task<Subscription?> GetByIdAsync(int id);
    Task<Subscription> AddAsync(Subscription subscription);
    Task<Subscription> UpdateAsync(Subscription subscription);
    Task DeleteAsync(int id);
}