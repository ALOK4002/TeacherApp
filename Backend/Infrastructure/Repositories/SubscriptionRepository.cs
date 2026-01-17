using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetByUserIdAsync(int userId)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);
    }

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subscription> AddAsync(Subscription subscription)
    {
        subscription.CreatedDate = DateTime.UtcNow;
        subscription.UpdatedDate = DateTime.UtcNow;
        
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription> UpdateAsync(Subscription subscription)
    {
        subscription.UpdatedDate = DateTime.UtcNow;
        
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task DeleteAsync(int id)
    {
        var subscription = await GetByIdAsync(id);
        if (subscription != null)
        {
            subscription.IsActive = false;
            subscription.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}