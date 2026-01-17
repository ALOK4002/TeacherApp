using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class NoticeRepository : INoticeRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<NoticeRepository> _logger;

    public NoticeRepository(AppDbContext context, ILogger<NoticeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Notice>> GetAllActiveNoticesAsync()
    {
        _logger.LogInformation("Entering GetAllActiveNoticesAsync");
        var query = _context.Notices
            .Include(n => n.PostedByUser)
            .Include(n => n.Replies.Where(r => r.IsActive))
            .Where(n => n.IsActive)
            .OrderByDescending(n => n.PostedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetAllActiveNoticesAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<Notice?> GetNoticeByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetNoticeByIdAsync with Id: {Id}", id);
        var query = _context.Notices
            .Include(n => n.PostedByUser)
            .Include(n => n.Replies.Where(r => r.IsActive))
            .ThenInclude(r => r.RepliedByUser)
            .Where(n => n.Id == id && n.IsActive);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetNoticeByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<Notice> AddNoticeAsync(Notice notice)
    {
        _logger.LogInformation("Entering AddNoticeAsync for Title: {Title}", notice.Title);
        notice.PostedDate = DateTime.UtcNow;
        notice.CreatedDate = DateTime.UtcNow;
        notice.UpdatedDate = DateTime.UtcNow;
        notice.IsActive = true;

        await _context.Notices.AddAsync(notice);
        _logger.LogInformation("SQL SaveChanges for AddNoticeAsync");
        await _context.SaveChangesAsync();
        
        // Load the user information
        await _context.Entry(notice)
            .Reference(n => n.PostedByUser)
            .LoadAsync();
        _logger.LogInformation("Exiting AddNoticeAsync with Id: {Id}", notice.Id);
        return notice;
    }

    public async Task<Notice> UpdateNoticeAsync(Notice notice)
    {
        _logger.LogInformation("Entering UpdateNoticeAsync for Id: {Id}", notice.Id);
        notice.UpdatedDate = DateTime.UtcNow;
        
        _context.Notices.Update(notice);
        _logger.LogInformation("SQL SaveChanges for UpdateNoticeAsync");
        await _context.SaveChangesAsync();
        
        // Load the user information
        await _context.Entry(notice)
            .Reference(n => n.PostedByUser)
            .LoadAsync();
        _logger.LogInformation("Exiting UpdateNoticeAsync for Id: {Id}", notice.Id);
        return notice;
    }

    public async Task<bool> DeleteNoticeAsync(int id)
    {
        _logger.LogInformation("Entering DeleteNoticeAsync for Id: {Id}", id);
        var notice = await _context.Notices.FindAsync(id);
        if (notice == null)
        {
            _logger.LogWarning("DeleteNoticeAsync: Notice not found for Id: {Id}", id);
            return false;
        }

        notice.IsActive = false;
        notice.UpdatedDate = DateTime.UtcNow;
        
        _logger.LogInformation("SQL SaveChanges for DeleteNoticeAsync (soft delete) Id: {Id}", id);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting DeleteNoticeAsync successfully for Id: {Id}", id);
        return true;
    }

    public async Task<IEnumerable<Notice>> GetNoticesByUserIdAsync(int userId)
    {
        _logger.LogInformation("Entering GetNoticesByUserIdAsync for UserId: {UserId}", userId);
        var query = _context.Notices
            .Include(n => n.PostedByUser)
            .Include(n => n.Replies.Where(r => r.IsActive))
            .Where(n => n.PostedByUserId == userId && n.IsActive)
            .OrderByDescending(n => n.PostedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetNoticesByUserIdAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<IEnumerable<NoticeReply>> GetRepliesForNoticeAsync(int noticeId, int noticeOwnerId)
    {
        _logger.LogInformation("Entering GetRepliesForNoticeAsync for NoticeId: {NoticeId}, OwnerId: {OwnerId}", noticeId, noticeOwnerId);
        // Only the notice owner can see all replies to their notice
        var notice = await _context.Notices.FindAsync(noticeId);
        if (notice == null || notice.PostedByUserId != noticeOwnerId)
        {
            _logger.LogWarning("GetRepliesForNoticeAsync: Access denied or not found for NoticeId: {NoticeId}", noticeId);
            return Enumerable.Empty<NoticeReply>();
        }

        var query = _context.NoticeReplies
            .Include(r => r.RepliedByUser)
            .Where(r => r.NoticeId == noticeId && r.IsActive)
            .OrderBy(r => r.RepliedDate);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetRepliesForNoticeAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<NoticeReply> AddReplyAsync(NoticeReply reply)
    {
        _logger.LogInformation("Entering AddReplyAsync for NoticeId: {NoticeId}", reply.NoticeId);
        reply.RepliedDate = DateTime.UtcNow;
        reply.CreatedDate = DateTime.UtcNow;
        reply.UpdatedDate = DateTime.UtcNow;
        reply.IsActive = true;

        await _context.NoticeReplies.AddAsync(reply);
        _logger.LogInformation("SQL SaveChanges for AddReplyAsync");
        await _context.SaveChangesAsync();
        
        // Load the user information
        await _context.Entry(reply)
            .Reference(r => r.RepliedByUser)
            .LoadAsync();
        _logger.LogInformation("Exiting AddReplyAsync with Id: {Id}", reply.Id);
        return reply;
    }
}