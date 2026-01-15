using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly AppDbContext _context;

    public UserProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByUserIdAsync(int userId)
    {
        return await _context.UserProfiles
            .Include(p => p.School)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<UserProfile?> GetByIdAsync(int id)
    {
        return await _context.UserProfiles
            .Include(p => p.School)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<UserProfile> AddAsync(UserProfile profile)
    {
        profile.CreatedDate = DateTime.UtcNow;
        profile.UpdatedDate = DateTime.UtcNow;
        await _context.UserProfiles.AddAsync(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<UserProfile> UpdateAsync(UserProfile profile)
    {
        profile.UpdatedDate = DateTime.UtcNow;
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var profile = await _context.UserProfiles.FindAsync(id);
        if (profile == null) return false;

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasProfileAsync(int userId)
    {
        return await _context.UserProfiles.AnyAsync(p => p.UserId == userId);
    }
}
