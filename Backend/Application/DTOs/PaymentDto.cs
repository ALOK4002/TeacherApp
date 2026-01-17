using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class PaymentDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;
    
    [JsonPropertyName("transactionId")]
    public string TransactionId { get; set; } = string.Empty;
    
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
    
    [JsonPropertyName("status")]
    public PaymentStatus Status { get; set; }
    
    [JsonPropertyName("gateway")]
    public PaymentGateway Gateway { get; set; }
    
    [JsonPropertyName("paymentDate")]
    public DateTime? PaymentDate { get; set; }
    
    [JsonPropertyName("approvedDate")]
    public DateTime? ApprovedDate { get; set; }
    
    [JsonPropertyName("rejectionReason")]
    public string? RejectionReason { get; set; }
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    // User details for admin view
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
    
    [JsonPropertyName("userEmail")]
    public string? UserEmail { get; set; }
    
    [JsonPropertyName("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;
    
    [JsonPropertyName("approvedByUserId")]
    public int? ApprovedByUserId { get; set; }
}

public class PaymentOrderResponseDto
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;
    
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
    
    [JsonPropertyName("merchantId")]
    public string MerchantId { get; set; } = string.Empty;
    
    [JsonPropertyName("checksumHash")]
    public string ChecksumHash { get; set; } = string.Empty;
    
    [JsonPropertyName("callbackUrl")]
    public string CallbackUrl { get; set; } = string.Empty;
    
    [JsonPropertyName("paytmUrl")]
    public string PaytmUrl { get; set; } = string.Empty;
}