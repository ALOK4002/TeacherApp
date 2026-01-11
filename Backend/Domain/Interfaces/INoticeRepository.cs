using Domain.Entities;

namespace Domain.Interfaces;

public interface INoticeRepository
{
    Task<IEnumerable<Notice>> GetAllActiveNoticesAsync();
    Task<Notice?> GetNoticeByIdAsync(int id);
    Task<Notice> AddNoticeAsync(Notice notice);
    Task<Notice> UpdateNoticeAsync(Notice notice);
    Task<bool> DeleteNoticeAsync(int id);
    Task<IEnumerable<Notice>> GetNoticesByUserIdAsync(int userId);
    Task<IEnumerable<NoticeReply>> GetRepliesForNoticeAsync(int noticeId, int noticeOwnerId);
    Task<NoticeReply> AddReplyAsync(NoticeReply reply);
}