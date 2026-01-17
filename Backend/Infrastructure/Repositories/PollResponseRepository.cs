using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PollResponseRepository : IPollResponseRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PollResponseRepository> _logger;

    public PollResponseRepository(AppDbContext context, ILogger<PollResponseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PollResponse?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetByIdAsync with Id: {Id}", id);
        var query = _context.PollResponses
            .Include(r => r.Poll)
            .Include(r => r.User)
            .Include(r => r.Answers)
                .ThenInclude(a => a.PollQuestion)
            .Include(r => r.Answers)
                .ThenInclude(a => a.PollOption)
            .Where(r => r.Id == id && r.IsActive);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<PollResponse> AddAsync(PollResponse response)
    {
        _logger.LogInformation("Entering AddAsync for PollId: {PollId}", response.PollId);
        response.RespondedDate = DateTime.UtcNow;
        response.IsActive = true;

        await _context.PollResponses.AddAsync(response);
        _logger.LogInformation("SQL SaveChanges for AddAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting AddAsync with Id: {Id}", response.Id);
        return response;
    }

    public async Task<PollResponse> UpdateAsync(PollResponse response)
    {
        _logger.LogInformation("Entering UpdateAsync for Id: {Id}", response.Id);
        // No UpdatedDate for PollResponse as it represents a point-in-time response
        
        _context.PollResponses.Update(response);
        _logger.LogInformation("SQL SaveChanges for UpdateAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting UpdateAsync for Id: {Id}", response.Id);
        return response;
    }

    public async Task<IEnumerable<PollResponse>> GetByPollIdAsync(int pollId)
    {
        _logger.LogInformation("Entering GetByPollIdAsync for PollId: {PollId}", pollId);
        var query = _context.PollResponses
            .Include(r => r.User)
            .Include(r => r.Answers)
                .ThenInclude(a => a.PollQuestion)
            .Include(r => r.Answers)
                .ThenInclude(a => a.PollOption)
            .Where(r => r.PollId == pollId && r.IsActive)
            .OrderByDescending(r => r.RespondedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetByPollIdAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<PollResponse?> GetUserResponseAsync(int pollId, int? userId, string? ipAddress)
    {
        _logger.LogInformation("Entering GetUserResponseAsync for PollId: {PollId}, UserId: {UserId}, IpAddress: {IpAddress}", pollId, userId, ipAddress);
        var query = _context.PollResponses
            .Include(r => r.Answers)
                .ThenInclude(a => a.PollQuestion)
            .Include(r => r.Answers)
                .ThenInclude(a => a.PollOption)
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
            _logger.LogInformation("Exiting GetUserResponseAsync with null (no user identification)");
            return null;
        }

        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetUserResponseAsync; found: {Found}", result != null);
        return result;
    }
}
