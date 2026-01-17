using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PollRepository : IPollRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PollRepository> _logger;

    public PollRepository(AppDbContext context, ILogger<PollRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Poll>> GetAllActiveAsync()
    {
        _logger.LogInformation("Entering GetAllActiveAsync");
        var query = _context.Polls
            .Include(p => p.CreatedByUser)
            .Include(p => p.Questions)
                .ThenInclude(q => q.Options)
            .Where(p => p.IsActive && (!p.EndDate.HasValue || p.EndDate >= DateTime.UtcNow))
            .OrderByDescending(p => p.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetAllActiveAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<Poll?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetByIdAsync with Id: {Id}", id);
        var query = _context.Polls
            .Include(p => p.CreatedByUser)
            .Include(p => p.Questions)
                .ThenInclude(q => q.Options)
            .Include(p => p.Responses)
                .ThenInclude(r => r.Answers)
            .Where(p => p.Id == id && p.IsActive);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<Poll> AddAsync(Poll poll)
    {
        _logger.LogInformation("Entering AddAsync for Title: {Title}", poll.Title);
        poll.CreatedDate = DateTime.UtcNow;
        poll.UpdatedDate = DateTime.UtcNow;
        poll.IsActive = true;

        await _context.Polls.AddAsync(poll);
        _logger.LogInformation("SQL SaveChanges for AddAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting AddAsync with Id: {Id}", poll.Id);
        return poll;
    }

    public async Task<Poll> UpdateAsync(Poll poll)
    {
        _logger.LogInformation("Entering UpdateAsync for Id: {Id}", poll.Id);
        poll.UpdatedDate = DateTime.UtcNow;
        
        _context.Polls.Update(poll);
        _logger.LogInformation("SQL SaveChanges for UpdateAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting UpdateAsync for Id: {Id}", poll.Id);
        return poll;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Entering DeleteAsync for Id: {Id}", id);
        var poll = await _context.Polls.FindAsync(id);
        if (poll == null)
        {
            _logger.LogWarning("DeleteAsync: Poll not found for Id: {Id}", id);
            return false;
        }

        poll.IsActive = false;
        poll.UpdatedDate = DateTime.UtcNow;
        
        _logger.LogInformation("SQL SaveChanges for DeleteAsync (soft delete) Id: {Id}", id);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting DeleteAsync successfully for Id: {Id}", id);
        return true;
    }

    public async Task<IEnumerable<Poll>> GetByUserIdAsync(int userId)
    {
        _logger.LogInformation("Entering GetByUserIdAsync for UserId: {UserId}", userId);
        var query = _context.Polls
            .Include(p => p.CreatedByUser)
            .Include(p => p.Questions)
                .ThenInclude(q => q.Options)
            .Where(p => p.CreatedByUserId == userId && p.IsActive)
            .OrderByDescending(p => p.CreatedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetByUserIdAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<bool> HasUserRespondedAsync(int pollId, int? userId, string? ipAddress)
    {
        _logger.LogInformation("Entering HasUserRespondedAsync for PollId: {PollId}, UserId: {UserId}, IpAddress: {IpAddress}", pollId, userId, ipAddress);
        var query = _context.PollResponses
            .Where(r => r.PollId == pollId && r.IsActive);
        
        if (userId.HasValue)
        {
            query = query.Where(r => r.UserId == userId);
        }
        else if (!string.IsNullOrEmpty(ipAddress))
        {
            query = query.Where(r => r.UserIpAddress == ipAddress && r.UserId == null);
        }
        else
        {
            _logger.LogInformation("Exiting HasUserRespondedAsync with false (no user identification)");
            return false;
        }

        var hasResponded = await query.AnyAsync();
        _logger.LogInformation("Exiting HasUserRespondedAsync with result: {Result}", hasResponded);
        return hasResponded;
    }
}
