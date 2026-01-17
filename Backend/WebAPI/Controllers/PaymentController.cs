using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreatePaymentOrder([FromBody] CreatePaymentOrderDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _paymentService.CreatePaymentOrderAsync(userId, dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating payment order" });
        }
    }

    [HttpPost("paytm/callback")]
    [AllowAnonymous]
    public async Task<IActionResult> PaytmCallback([FromForm] PaymentCallbackDto dto)
    {
        try
        {
            var result = await _paymentService.HandlePaymentCallbackAsync(dto);
            
            // Redirect to frontend with payment status
            var redirectUrl = $"/payment-result?status={result.Status}&orderId={result.OrderId}";
            return Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            var redirectUrl = $"/payment-result?status=failed&error={Uri.EscapeDataString(ex.Message)}";
            return Redirect(redirectUrl);
        }
    }

    [HttpPost("approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApprovePayment(int id)
    {
        try
        {
            var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _paymentService.ApprovePaymentAsync(id, adminUserId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while approving payment" });
        }
    }

    [HttpPost("reject/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RejectPayment(int id, [FromBody] RejectPaymentDto dto)
    {
        try
        {
            var adminUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _paymentService.RejectPaymentAsync(id, adminUserId, dto.RejectionReason);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while rejecting payment" });
        }
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPendingPayments()
    {
        try
        {
            var payments = await _paymentService.GetPendingPaymentsAsync();
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching pending payments" });
        }
    }

    [HttpGet("my-payments")]
    public async Task<IActionResult> GetMyPayments()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var payments = await _paymentService.GetUserPaymentsAsync(userId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching payments" });
        }
    }
}

public class RejectPaymentDto
{
    public string RejectionReason { get; set; } = string.Empty;
}