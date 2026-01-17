using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreatePaymentOrder([FromBody] CreatePaymentOrderDto dto)
    {
        _logger.LogInformation("Entering CreatePaymentOrder for UserId: {UserId}, SubscriptionTier: {SubscriptionTier}, Amount: {Amount}", 
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"), dto.SubscriptionTier, dto.Amount);
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _paymentService.CreatePaymentOrderAsync(userId, dto);
            _logger.LogInformation("Exiting CreatePaymentOrder successfully for OrderId: {OrderId}", result.OrderId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "CreatePaymentOrder failed");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreatePaymentOrder error");
            return StatusCode(500, new { message = "An error occurred while creating payment order" });
        }
    }

    [HttpPost("paytm/callback")]
    [AllowAnonymous]
    public async Task<IActionResult> PaytmCallback([FromForm] PaymentCallbackDto dto)
    {
        _logger.LogInformation("Entering PaytmCallback for OrderId: {OrderId}", dto.OrderId);
        try
        {
            var result = await _paymentService.HandlePaymentCallbackAsync(dto);
            
            // Redirect to frontend with payment status
            var redirectUrl = $"/payment-result?status={result.Status}&orderId={result.OrderId}";
            _logger.LogInformation("Exiting PaytmCallback redirecting to: {RedirectUrl}", redirectUrl);
            return Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            var redirectUrl = $"/payment-result?status=failed&error={Uri.EscapeDataString(ex.Message)}";
            _logger.LogError(ex, "PaytmCallback error redirecting to: {RedirectUrl}", redirectUrl);
            return Redirect(redirectUrl);
        }
    }

    [HttpPost("approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApprovePayment(int id)
    {
        _logger.LogInformation("Entering ApprovePayment for PaymentId: {PaymentId}", id);
        try
        {
            var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _paymentService.ApprovePaymentAsync(id, adminUserId);
            _logger.LogInformation("Exiting ApprovePayment successfully for PaymentId: {PaymentId}", id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "ApprovePayment failed for PaymentId: {PaymentId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ApprovePayment error for PaymentId: {PaymentId}", id);
            return StatusCode(500, new { message = "An error occurred while approving payment" });
        }
    }

    [HttpPost("reject/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RejectPayment(int id, [FromBody] RejectPaymentDto dto)
    {
        _logger.LogInformation("Entering RejectPayment for PaymentId: {PaymentId}", id);
        try
        {
            var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _paymentService.RejectPaymentAsync(id, adminUserId, dto.RejectionReason);
            _logger.LogInformation("Exiting RejectPayment successfully for PaymentId: {PaymentId}", id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "RejectPayment failed for PaymentId: {PaymentId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RejectPayment error for PaymentId: {PaymentId}", id);
            return StatusCode(500, new { message = "An error occurred while rejecting payment" });
        }
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPendingPayments()
    {
        _logger.LogInformation("Entering GetPendingPayments");
        try
        {
            var payments = await _paymentService.GetPendingPaymentsAsync();
            _logger.LogInformation("Exiting GetPendingPayments with count: {Count}", ((IEnumerable<object>?)payments)?.Count() ?? 0);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPendingPayments error");
            return StatusCode(500, new { message = "An error occurred while fetching pending payments" });
        }
    }

    [HttpGet("my-payments")]
    public async Task<IActionResult> GetMyPayments()
    {
        _logger.LogInformation("Entering GetMyPayments");
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var payments = await _paymentService.GetUserPaymentsAsync(userId);
            _logger.LogInformation("Exiting GetMyPayments with count: {Count}", ((IEnumerable<object>?)payments)?.Count() ?? 0);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetMyPayments error");
            return StatusCode(500, new { message = "An error occurred while fetching payments" });
        }
    }
}

public class RejectPaymentDto
{
    public string RejectionReason { get; set; } = string.Empty;
}