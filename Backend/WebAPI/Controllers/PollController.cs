using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PollController : ControllerBase
{
    private readonly IPollService _pollService;
    private readonly ILogger<PollController> _logger;

    public PollController(IPollService pollService, ILogger<PollController> logger)
    {
        _pollService = pollService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActivePolls()
    {
        _logger.LogInformation("Entering GetAllActivePolls");
        try
        {
            var polls = await _pollService.GetAllActivePollsAsync();
            _logger.LogInformation("Exiting GetAllActivePolls with count: {Count}", polls.Count());
            return Ok(polls);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllActivePolls error");
            return StatusCode(500, new { message = "An error occurred while retrieving polls" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPollById(int id)
    {
        _logger.LogInformation("Entering GetPollById for Id: {Id}", id);
        try
        {
            var poll = await _pollService.GetPollByIdAsync(id);
            if (poll == null)
            {
                _logger.LogWarning("GetPollById not found for Id: {Id}", id);
                return NotFound(new { message = "Poll not found" });
            }
            _logger.LogInformation("Exiting GetPollById successfully for Id: {Id}", id);
            return Ok(poll);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPollById error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the poll" });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePoll([FromBody] CreatePollDto dto)
    {
        _logger.LogInformation("Entering CreatePoll with Title: {Title}", dto.Title);
        try
        {
            var validator = new CreatePollValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("CreatePoll validation failed for Title: {Title}", dto.Title);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var userId = GetCurrentUserId();
            var poll = await _pollService.CreatePollAsync(dto, userId);
            _logger.LogInformation("Exiting CreatePoll successfully with Id: {Id}", poll.Id);
            return CreatedAtAction(nameof(GetPollById), new { id = poll.Id }, poll);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreatePoll error for Title: {Title}", dto.Title);
            return StatusCode(500, new { message = "An error occurred while creating the poll" });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdatePoll(int id, [FromBody] UpdatePollDto dto)
    {
        _logger.LogInformation("Entering UpdatePoll for Id: {Id}", id);
        try
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("UpdatePoll ID mismatch: PathId={PathId}, BodyId={BodyId}", id, dto.Id);
                return BadRequest(new { message = "ID mismatch" });
            }

            var validator = new UpdatePollValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("UpdatePoll validation failed for Id: {Id}", id);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var poll = await _pollService.UpdatePollAsync(dto);
            _logger.LogInformation("Exiting UpdatePoll successfully for Id: {Id}", id);
            return Ok(poll);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "UpdatePoll not found for Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdatePoll error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the poll" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePoll(int id)
    {
        _logger.LogInformation("Entering DeletePoll for Id: {Id}", id);
        try
        {
            var userId = GetCurrentUserId();
            var result = await _pollService.DeletePollAsync(id, userId);
            if (!result)
            {
                _logger.LogWarning("DeletePoll failed to find or authorize Id: {Id}", id);
                return NotFound(new { message = "Poll not found or access denied" });
            }
            _logger.LogInformation("Exiting DeletePoll successfully for Id: {Id}", id);
            return Ok(new { message = "Poll deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeletePoll error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the poll" });
        }
    }

    [HttpGet("my-polls")]
    [Authorize]
    public async Task<IActionResult> GetUserPolls()
    {
        _logger.LogInformation("Entering GetUserPolls");
        try
        {
            var userId = GetCurrentUserId();
            var polls = await _pollService.GetUserPollsAsync(userId);
            _logger.LogInformation("Exiting GetUserPolls with count: {Count}", polls.Count());
            return Ok(polls);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserPolls error");
            return StatusCode(500, new { message = "An error occurred while retrieving your polls" });
        }
    }

    [HttpGet("{id}/results")]
    public async Task<IActionResult> GetPollResults(int id)
    {
        _logger.LogInformation("Entering GetPollResults for Id: {Id}", id);
        try
        {
            var results = await _pollService.GetPollResultsAsync(id);
            _logger.LogInformation("Exiting GetPollResults successfully for Id: {Id}", id);
            return Ok(results);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "GetPollResults not found for Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPollResults error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving poll results" });
        }
    }

    [HttpPost("{id}/respond")]
    public async Task<IActionResult> SubmitPollResponse(int id, [FromBody] SubmitPollResponseDto dto)
    {
        _logger.LogInformation("Entering SubmitPollResponse for PollId: {PollId}", id);
        try
        {
            if (id != dto.PollId)
            {
                _logger.LogWarning("SubmitPollResponse ID mismatch: PathId={PathId}, BodyId={BodyId}", id, dto.PollId);
                return BadRequest(new { message = "ID mismatch" });
            }

            var validator = new SubmitPollResponseValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("SubmitPollResponse validation failed for PollId: {PollId}", id);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var userId = User.Identity.IsAuthenticated ? GetCurrentUserId() : (int?)null;
            var ipAddress = GetClientIpAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var response = await _pollService.SubmitPollResponseAsync(dto, userId, ipAddress, userAgent);
            _logger.LogInformation("Exiting SubmitPollResponse successfully for PollId: {PollId}", id);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "SubmitPollResponse failed for PollId: {PollId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SubmitPollResponse error for PollId: {PollId}", id);
            return StatusCode(500, new { message = "An error occurred while submitting your response" });
        }
    }

    [HttpGet("{id}/my-response")]
    public async Task<IActionResult> GetUserPollResponse(int id)
    {
        _logger.LogInformation("Entering GetUserPollResponse for PollId: {PollId}", id);
        try
        {
            var userId = User.Identity.IsAuthenticated ? GetCurrentUserId() : (int?)null;
            var ipAddress = GetClientIpAddress();

            var response = await _pollService.GetUserPollResponseAsync(id, userId, ipAddress);
            _logger.LogInformation("Exiting GetUserPollResponse; found: {Found}", response != null);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserPollResponse error for PollId: {PollId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving your response" });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    private string? GetClientIpAddress()
    {
        // Try to get real IP from forwarded headers
        var ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(ipAddress))
        {
            return ipAddress.Split(',')[0].Trim();
        }

        ipAddress = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(ipAddress))
        {
            return ipAddress;
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
