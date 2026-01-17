using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class SubscriptionDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("tier")]
    public SubscriptionTier Tier { get; set; }
    
    [JsonPropertyName("status")]
    public SubscriptionStatus Status { get; set; }
    
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
    
    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; set; }
    
    [JsonPropertyName("documentUploadLimit")]
    public int DocumentUploadLimit { get; set; }
    
    [JsonPropertyName("fileSizeLimitInBytes")]
    public long FileSizeLimitInBytes { get; set; }
    
    [JsonPropertyName("fileSizeLimitFormatted")]
    public string FileSizeLimitFormatted { get; set; } = string.Empty;
    
    [JsonPropertyName("documentsUploaded")]
    public int DocumentsUploaded { get; set; }
    
    [JsonPropertyName("remainingUploads")]
    public int RemainingUploads { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
}

public class CreatePaymentOrderDto
{
    [JsonPropertyName("subscriptionTier")]
    public SubscriptionTier SubscriptionTier { get; set; }
    
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "INR";
}

public class PaymentCallbackDto
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;
    
    [JsonPropertyName("transactionId")]
    public string TransactionId { get; set; } = string.Empty;
    
    [JsonPropertyName("amount")]
    public string Amount { get; set; } = string.Empty;
    
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("checksumhash")]
    public string ChecksumHash { get; set; } = string.Empty;
    
    [JsonPropertyName("gatewayResponse")]
    public string GatewayResponse { get; set; } = string.Empty;
}