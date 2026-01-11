using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class NoticeService : INoticeService
{
    private readonly INoticeRepository _noticeRepository;

    public NoticeService(INoticeRepository noticeRepository)
    {
        _noticeRepository = noticeRepository;
    }

    public async Task<IEnumerable<NoticeDto>> GetAllActiveNoticesAsync(int currentUserId)
    {
        var notices = await _noticeRepository.GetAllActiveNoticesAsync();
        return notices.Select(n => MapToDto(n, currentUserId));
    }

    public async Task<NoticeWithRepliesDto?> GetNoticeWithRepliesAsync(int noticeId, int currentUserId)
    {
        var notice = await _noticeRepository.GetNoticeByIdAsync(noticeId);
        if (notice == null) return null;

        var noticeDto = MapToDto(notice, currentUserId);
        var repliesDto = new List<NoticeReplyDto>();

        // Only the notice owner can see replies
        if (notice.PostedByUserId == currentUserId)
        {
            var replies = await _noticeRepository.GetRepliesForNoticeAsync(noticeId, currentUserId);
            repliesDto = replies.Select(MapReplyToDto).ToList();
        }

        return new NoticeWithRepliesDto
        {
            Notice = noticeDto,
            Replies = repliesDto
        };
    }

    public async Task<NoticeDto> CreateNoticeAsync(CreateNoticeDto dto, int userId, string userName)
    {
        var notice = new Notice
        {
            Title = dto.Title,
            Message = dto.Message,
            Category = dto.Category,
            Priority = dto.Priority,
            PostedByUserId = userId,
            PostedByUserName = userName
        };

        var createdNotice = await _noticeRepository.AddNoticeAsync(notice);
        return MapToDto(createdNotice, userId);
    }

    public async Task<NoticeDto> UpdateNoticeAsync(UpdateNoticeDto dto, int currentUserId)
    {
        var existingNotice = await _noticeRepository.GetNoticeByIdAsync(dto.Id);
        if (existingNotice == null)
        {
            throw new InvalidOperationException("Notice not found");
        }

        // Only the notice owner can update their notice
        if (existingNotice.PostedByUserId != currentUserId)
        {
            throw new UnauthorizedAccessException("You can only update your own notices");
        }

        existingNotice.Title = dto.Title;
        existingNotice.Message = dto.Message;
        existingNotice.Category = dto.Category;
        existingNotice.Priority = dto.Priority;
        existingNotice.IsActive = dto.IsActive;

        var updatedNotice = await _noticeRepository.UpdateNoticeAsync(existingNotice);
        return MapToDto(updatedNotice, currentUserId);
    }

    public async Task<bool> DeleteNoticeAsync(int noticeId, int currentUserId)
    {
        var notice = await _noticeRepository.GetNoticeByIdAsync(noticeId);
        if (notice == null) return false;

        // Only the notice owner can delete their notice
        if (notice.PostedByUserId != currentUserId)
        {
            throw new UnauthorizedAccessException("You can only delete your own notices");
        }

        return await _noticeRepository.DeleteNoticeAsync(noticeId);
    }

    public async Task<IEnumerable<NoticeDto>> GetMyNoticesAsync(int userId)
    {
        var notices = await _noticeRepository.GetNoticesByUserIdAsync(userId);
        return notices.Select(n => MapToDto(n, userId));
    }

    public async Task<NoticeReplyDto> AddReplyAsync(CreateNoticeReplyDto dto, int userId, string userName)
    {
        var notice = await _noticeRepository.GetNoticeByIdAsync(dto.NoticeId);
        if (notice == null)
        {
            throw new InvalidOperationException("Notice not found");
        }

        var reply = new NoticeReply
        {
            NoticeId = dto.NoticeId,
            ReplyMessage = dto.ReplyMessage,
            RepliedByUserId = userId,
            RepliedByUserName = userName
        };

        var createdReply = await _noticeRepository.AddReplyAsync(reply);
        return MapReplyToDto(createdReply);
    }

    private static NoticeDto MapToDto(Notice notice, int currentUserId)
    {
        // Check if current user has replied to this notice
        var hasReplied = notice.Replies.Any(r => r.RepliedByUserId == currentUserId && r.IsActive);

        return new NoticeDto
        {
            Id = notice.Id,
            Title = notice.Title,
            Message = notice.Message,
            Category = notice.Category,
            Priority = notice.Priority,
            PostedByUserId = notice.PostedByUserId,
            PostedByUserName = notice.PostedByUserName,
            PostedDate = notice.PostedDate,
            IsActive = notice.IsActive,
            ReplyCount = notice.Replies.Count(r => r.IsActive),
            HasReplied = hasReplied
        };
    }

    private static NoticeReplyDto MapReplyToDto(NoticeReply reply)
    {
        return new NoticeReplyDto
        {
            Id = reply.Id,
            NoticeId = reply.NoticeId,
            ReplyMessage = reply.ReplyMessage,
            RepliedByUserId = reply.RepliedByUserId,
            RepliedByUserName = reply.RepliedByUserName,
            RepliedDate = reply.RepliedDate,
            IsActive = reply.IsActive
        };
    }
}