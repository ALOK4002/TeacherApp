using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserActivityController : ControllerBase
{
    private readonly IUserActivityService _activityService;

    public UserActivityController(IUserActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet("my-activities")]
    public async Task<IActionResult> GetMyActivities([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var activities = await _activityService.GetUserActivitiesAsync(userId, page, pageSize);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching activities" });
        }
    }
}