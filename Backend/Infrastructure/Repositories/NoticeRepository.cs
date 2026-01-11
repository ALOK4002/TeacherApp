using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NoticeRepository : INoticeRepository
{
    private readonly AppDbContext _context;

    public NoticeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notice>> GetAllActiveNoticesAsync()
    {
        return await _context.Notices
            .Include(n => n.PostedByUser)
            .Include(n => n.Replies.Where(r => r.IsActive))
            .Where(n => n.IsActive)
            .OrderByDescending(n => n.PostedDate)
            .ToListAsync();
    }

    public async Task<Notice?> GetNoticeByIdAsync(int id)
    {
        return await _context.Notices
            .Include(n => n.PostedByUser)
            .Include(n => n.Replies.Where(r => r.IsActive))
            .ThenInclude(r => r.RepliedByUser)
            .FirstOrDefaultAsync(n => n.Id == id && n.IsActive);
    }

    public async Task<Notice> AddNoticeAsync(Notice notice)
    {
        notice.PostedDate = DateTime.UtcNow;
        notice.CreatedDate = DateTime.UtcNow;
        notice.UpdatedDate = DateTime.UtcNow;
        notice.IsActive = true;

        await _context.Notices.AddAsync(notice);
        await _context.SaveChangesAsync();
        
        // Load the user information
        await _context.Entry(notice)
            .Reference(n => n.PostedByUser)
            .LoadAsync();
            
        return notice;
    }

    public async Task<Notice> UpdateNoticeAsync(Notice notice)
    {
        notice.UpdatedDate = DateTime.UtcNow;
        
        _context.Notices.Update(notice);
        await _context.SaveChangesAsync();
        
        // Load the user information
        await _context.Entry(notice)
            .Reference(n => n.PostedByUser)
            .LoadAsync();
            
        return notice;
    }

    public async Task<bool> DeleteNoticeAsync(int id)
    {
        var notice = await _context.Notices.FindAsync(id);
        if (notice == null) return false;

        notice.IsActive = false;
        notice.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Notice>> GetNoticesByUserIdAsync(int userId)
    {
        return await _context.Notices
            .Include(n => n.PostedByUser)
            .Include(n => n.Replies.Where(r => r.IsActive))
            .Where(n => n.PostedByUserId == userId && n.IsActive)
            .OrderByDescending(n => n.PostedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<NoticeReply>> GetRepliesForNoticeAsync(int noticeId, int noticeOwnerId)
    {
        // Only the notice owner can see all replies to their notice
        var notice = await _context.Notices.FindAsync(noticeId);
        if (notice == null || notice.PostedByUserId != noticeOwnerId)
        {
            return Enumerable.Empty<NoticeReply>();
        }

        return await _context.NoticeReplies
            .Include(r => r.RepliedByUser)
            .Where(r => r.NoticeId == noticeId && r.IsActive)
            .OrderBy(r => r.RepliedDate)
            .ToListAsync();
    }

    public async Task<NoticeReply> AddReplyAsync(NoticeReply reply)
    {
        reply.RepliedDate = DateTime.UtcNow;
        reply.CreatedDate = DateTime.UtcNow;
        reply.UpdatedDate = DateTime.UtcNow;
        reply.IsActive = true;

        await _context.NoticeReplies.AddAsync(reply);
        await _context.SaveChangesAsync();
        
        // Load the user information
        await _context.Entry(reply)
            .Reference(r => r.RepliedByUser)
            .LoadAsync();
            
        return reply;
    }
}