using Application.DTOs;

namespace Application.Interfaces;

public interface INoticeService
{
    Task<IEnumerable<NoticeDto>> GetAllActiveNoticesAsync(int currentUserId);
    Task<NoticeWithRepliesDto?> GetNoticeWithRepliesAsync(int noticeId, int currentUserId);
    Task<NoticeDto> CreateNoticeAsync(CreateNoticeDto dto, int userId, string userName);
    Task<NoticeDto> UpdateNoticeAsync(UpdateNoticeDto dto, int currentUserId);
    Task<bool> DeleteNoticeAsync(int noticeId, int currentUserId);
    Task<IEnumerable<NoticeDto>> GetMyNoticesAsync(int userId);
    Task<NoticeReplyDto> AddReplyAsync(CreateNoticeReplyDto dto, int userId, string userName);
}