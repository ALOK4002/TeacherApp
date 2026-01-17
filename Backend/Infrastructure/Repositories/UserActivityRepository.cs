using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserActivityRepository : IUserActivityRepository
{
    private readonly AppDbContext _context;

    public UserActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserActivity?> GetByIdAsync(int id)
    {
        return await _context.UserActivities
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<UserActivity>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 20)
    {
        return await _context.UserActivities
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderByDescending(a => a.ActivityDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<UserActivity> AddAsync(UserActivity activity)
    {
        activity.ActivityDate = DateTime.UtcNow;
        
        _context.UserActivities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<UserActivity> UpdateAsync(UserActivity activity)
    {
        _context.UserActivities.Update(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task DeleteAsync(int id)
    {
        var activity = await GetByIdAsync(id);
        if (activity != null)
        {
            activity.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}