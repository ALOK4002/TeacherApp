using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoticeController : ControllerBase
{
    private readonly INoticeService _noticeService;

    public NoticeController(INoticeService noticeService)
    {
        _noticeService = noticeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotices()
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var notices = await _noticeService.GetAllActiveNoticesAsync(currentUserId);
            return Ok(notices);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving notices" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoticeWithReplies(int id)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var noticeWithReplies = await _noticeService.GetNoticeWithRepliesAsync(id, currentUserId);
            
            if (noticeWithReplies == null)
            {
                return NotFound(new { message = "Notice not found" });
            }
            
            return Ok(noticeWithReplies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the notice" });
        }
    }

    [HttpGet("my-notices")]
    public async Task<IActionResult> GetMyNotices()
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var notices = await _noticeService.GetMyNoticesAsync(currentUserId);
            return Ok(notices);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving your notices" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotice([FromBody] CreateNoticeDto dto)
    {
        try
        {
            var validator = new CreateNoticeValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var currentUserId = GetCurrentUserId();
            var currentUserName = GetCurrentUserName();
            
            var notice = await _noticeService.CreateNoticeAsync(dto, currentUserId, currentUserName);
            return CreatedAtAction(nameof(GetNoticeWithReplies), new { id = notice.Id }, notice);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the notice" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNotice(int id, [FromBody] UpdateNoticeDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var currentUserId = GetCurrentUserId();
            var notice = await _noticeService.UpdateNoticeAsync(dto, currentUserId);
            return Ok(notice);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the notice" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotice(int id)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var result = await _noticeService.DeleteNoticeAsync(id, currentUserId);
            
            if (!result)
            {
                return NotFound(new { message = "Notice not found" });
            }
            
            return Ok(new { message = "Notice deleted successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the notice" });
        }
    }

    [HttpPost("reply")]
    public async Task<IActionResult> AddReply([FromBody] CreateNoticeReplyDto dto)
    {
        try
        {
            var validator = new CreateNoticeReplyValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var currentUserId = GetCurrentUserId();
            var currentUserName = GetCurrentUserName();
            
            var reply = await _noticeService.AddReplyAsync(dto, currentUserId, currentUserName);
            return Ok(reply);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
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