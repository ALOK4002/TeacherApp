using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("my-subscription")]
    public async Task<IActionResult> GetMySubscription()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);
            
            if (subscription == null)
            {
                // Create free subscription if not exists
                subscription = await _subscriptionService.CreateFreeSubscriptionAsync(userId);
            }
            
            return Ok(subscription);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching subscription" });
        }
    }

    [HttpGet("can-upload")]
    public async Task<IActionResult> CanUploadDocument([FromQuery] long fileSizeInBytes)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var canUpload = await _subscriptionService.CanUploadDocumentAsync(userId, fileSizeInBytes);
            
            var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);
            
            return Ok(new
            {
                canUpload,
                subscription?.DocumentsUploaded,
                subscription?.DocumentUploadLimit,
                subscription?.RemainingUploads,
                subscription?.FileSizeLimitInBytes,
                subscription?.FileSizeLimitFormatted,
                subscription?.Tier
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while checking upload eligibility" });
        }
    }

    [HttpPost("increment-document-count")]
    public async Task<IActionResult> IncrementDocumentCount()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _subscriptionService.IncrementDocumentCountAsync(userId);
            return Ok(new { message = "Document count incremented successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating document count" });
        }
    }
}