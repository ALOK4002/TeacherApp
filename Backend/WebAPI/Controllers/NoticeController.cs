using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoticeController : ControllerBase
{
    private readonly INoticeService _noticeService;
    private readonly ILogger<NoticeController> _logger;

    public NoticeController(INoticeService noticeService, ILogger<NoticeController> logger)
    {
        _noticeService = noticeService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotices()
    {
        _logger.LogInformation("Entering GetAllNotices");
        try
        {
            var currentUserId = GetCurrentUserId();
            var notices = await _noticeService.GetAllActiveNoticesAsync(currentUserId);
            _logger.LogInformation("Exiting GetAllNotices with count: {Count}", ((IEnumerable<object>?)notices)?.Count() ?? 0);
            return Ok(notices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllNotices error");
            return StatusCode(500, new { message = "An error occurred while retrieving notices" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoticeWithReplies(int id)
    {
        _logger.LogInformation("Entering GetNoticeWithReplies for Id: {Id}", id);
        try
        {
            var currentUserId = GetCurrentUserId();
            var noticeWithReplies = await _noticeService.GetNoticeWithRepliesAsync(id, currentUserId);
            
            if (noticeWithReplies == null)
            {
                _logger.LogWarning("GetNoticeWithReplies: Notice not found for Id: {Id}", id);
                return NotFound(new { message = "Notice not found" });
            }
            
            _logger.LogInformation("Exiting GetNoticeWithReplies successfully for Id: {Id}", id);
            return Ok(noticeWithReplies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetNoticeWithReplies error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the notice" });
        }
    }

    [HttpGet("my-notices")]
    public async Task<IActionResult> GetMyNotices()
    {
        _logger.LogInformation("Entering GetMyNotices");
        try
        {
            var currentUserId = GetCurrentUserId();
            var notices = await _noticeService.GetMyNoticesAsync(currentUserId);
            _logger.LogInformation("Exiting GetMyNotices with count: {Count}", ((IEnumerable<object>?)notices)?.Count() ?? 0);
            return Ok(notices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetMyNotices error");
            return StatusCode(500, new { message = "An error occurred while retrieving your notices" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotice([FromBody] CreateNoticeDto dto)
    {
        _logger.LogInformation("Entering CreateNotice with Title: {Title}", dto.Title);
        try
        {
            var validator = new CreateNoticeValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("CreateNotice validation failed for Title: {Title}", dto.Title);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var currentUserId = GetCurrentUserId();
            var currentUserName = GetCurrentUserName();
            
            var notice = await _noticeService.CreateNoticeAsync(dto, currentUserId, currentUserName);
            _logger.LogInformation("Exiting CreateNotice successfully with Id: {Id}", notice.Id);
            return CreatedAtAction(nameof(GetNoticeWithReplies), new { id = notice.Id }, notice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateNotice error for Title: {Title}", dto.Title);
            return StatusCode(500, new { message = "An error occurred while creating the notice" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNotice(int id, [FromBody] UpdateNoticeDto dto)
    {
        _logger.LogInformation("Entering UpdateNotice for Id: {Id}", id);
        try
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("UpdateNotice ID mismatch: PathId={PathId}, BodyId={BodyId}", id, dto.Id);
                return BadRequest(new { message = "ID mismatch" });
            }

            var currentUserId = GetCurrentUserId();
            var notice = await _noticeService.UpdateNoticeAsync(dto, currentUserId);
            _logger.LogInformation("Exiting UpdateNotice successfully for Id: {Id}", id);
            return Ok(notice);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "UpdateNotice not found for Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "UpdateNotice unauthorized for Id: {Id}", id);
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateNotice error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the notice" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotice(int id)
    {
        _logger.LogInformation("Entering DeleteNotice for Id: {Id}", id);
        try
        {
            var currentUserId = GetCurrentUserId();
            var result = await _noticeService.DeleteNoticeAsync(id, currentUserId);
            
            if (!result)
            {
                _logger.LogWarning("DeleteNotice failed to find or authorize Id: {Id}", id);
                return NotFound(new { message = "Notice not found" });
            }
            
            _logger.LogInformation("Exiting DeleteNotice successfully for Id: {Id}", id);
            return Ok(new { message = "Notice deleted successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "DeleteNotice unauthorized for Id: {Id}", id);
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteNotice error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the notice" });
        }
    }

    [HttpPost("reply")]
    public async Task<IActionResult> AddReply([FromBody] CreateNoticeReplyDto dto)
    {
        _logger.LogInformation("Entering AddReply for NoticeId: {NoticeId}", dto.NoticeId);
        try
        {
            var validator = new CreateNoticeReplyValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("AddReply validation failed for NoticeId: {NoticeId}", dto.NoticeId);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var currentUserId = GetCurrentUserId();
            var currentUserName = GetCurrentUserName();
            
            var reply = await _noticeService.AddReplyAsync(dto, currentUserId, currentUserName);
            _logger.LogInformation("Exiting AddReply successfully with Id: {Id}", reply.Id);
            return Ok(reply);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "AddReply failed for NoticeId: {NoticeId}", dto.NoticeId);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddReply error for NoticeId: {NoticeId}", dto.NoticeId);
            return StatusCode(500, new { message = "An error occurred while adding the reply" });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    private string GetCurrentUserName()
    {
        return User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User";
    }
}