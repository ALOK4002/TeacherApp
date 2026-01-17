using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByUserNameOrEmailAsync(string value)
    {
        _logger.LogInformation("Entering GetByUserNameOrEmailAsync with Value: {Value}", value);
        var query = _context.Users
            .Where(u => u.UserName == value || u.Email == value);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByUserNameOrEmailAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetByIdAsync with Id: {Id}", id);
        var result = await _context.Users.FindAsync(id);
        _logger.LogInformation("Exiting GetByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task AddAsync(User user)
    {
        _logger.LogInformation("Entering AddAsync for Username: {Username}", user.UserName);
        user.CreatedDate = DateTime.UtcNow;
        user.UpdatedDate = DateTime.UtcNow;
        await _context.Users.AddAsync(user);
        _logger.LogInformation("SQL SaveChanges for AddAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting AddAsync with Id: {Id}", user.Id);
    }

    public async Task<User> UpdateAsync(User user)
    {
        _logger.LogInformation("Entering UpdateAsync for Id: {Id}", user.Id);
        user.UpdatedDate = DateTime.UtcNow;
        _context.Users.Update(user);
        _logger.LogInformation("SQL SaveChanges for UpdateAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting UpdateAsync for Id: {Id}", user.Id);
        return user;
    }

    public async Task<List<User>> GetPendingUsersAsync()
    {
        _logger.LogInformation("Entering GetPendingUsersAsync");
        var query = _context.Users
            .Where(u => !u.IsApproved && u.IsActive)
            .OrderBy(u => u.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetPendingUsersAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        _logger.LogInformation("Entering GetAllUsersAsync");
        var query = _context.Users
            .OrderByDescending(u => u.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetAllUsersAsync with count: {Count}", result.Count);
        return result;
    }
}