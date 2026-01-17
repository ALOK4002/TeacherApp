using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        _logger.LogInformation("Entering Register with Username: {Username}", dto.UserName);
        try
        {
            await _authService.RegisterAsync(dto);
            _logger.LogInformation("Exiting Register successfully for Username: {Username}", dto.UserName);
            return Ok(new { message = "Registration successful! Please wait for admin approval." });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Register failed for Username: {Username}", dto.UserName);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Register error for Username: {Username}", dto.UserName);
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        _logger.LogInformation("Entering Login with Username: {Username}", dto.UserName);
        try
        {
            var response = await _authService.LoginAsync(dto);
            _logger.LogInformation("Exiting Login successfully for Username: {Username}", dto.UserName);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Login failed for Username: {Username}", dto.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error for Username: {Username}", dto.UserName);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("pending-users")]
    public async Task<IActionResult> GetPendingUsers()
    {
        _logger.LogInformation("Entering GetPendingUsers");
        try
        {
            var response = await _authService.GetPendingUsersAsync();
            _logger.LogInformation("Exiting GetPendingUsers with count: {Count}", ((IEnumerable<object>?)response)?.Count() ?? 0);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPendingUsers error");
            return StatusCode(500, new { message = "An error occurred while fetching pending users" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("Entering GetAllUsers");
        try
        {
            var users = await _authService.GetAllUsersAsync();
            _logger.LogInformation("Exiting GetAllUsers with count: {Count}", ((IEnumerable<object>?)users)?.Count() ?? 0);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllUsers error");
            return StatusCode(500, new { message = "An error occurred while fetching users" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("approve/{userId}")]
    public async Task<IActionResult> ApproveUser(int userId)
    {
        _logger.LogInformation("Entering ApproveUser for UserId: {UserId}", userId);
        try
        {
            var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _authService.ApproveUserAsync(userId, adminUserId);
            _logger.LogInformation("Exiting ApproveUser successfully for UserId: {UserId}", userId);
            return Ok(new { message = "User approved successfully" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "ApproveUser failed for UserId: {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ApproveUser error for UserId: {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while approving user" });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("reject/{userId}")]
    public async Task<IActionResult> RejectUser(int userId, [FromBody] ApproveUserDto dto)
    {
        _logger.LogInformation("Entering RejectUser for UserId: {UserId}", userId);
        try
        {
            var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _authService.RejectUserAsync(userId, dto.RejectionReason ?? "No reason provided", adminUserId);
            _logger.LogInformation("Exiting RejectUser successfully for UserId: {UserId}", userId);
            return Ok(new { message = "User rejected successfully" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "RejectUser failed for UserId: {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RejectUser error for UserId: {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while rejecting user" });
        }
    }
}