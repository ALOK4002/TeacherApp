namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SubscriptionId { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public PaymentGateway Gateway { get; set; } = PaymentGateway.Paytm;
    public string GatewayOrderId { get; set; } = string.Empty;
    public string GatewayTransactionId { get; set; } = string.Empty;
    public string GatewayResponse { get; set; } = string.Empty;
    public string ChecksumHash { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public int? ApprovedByUserId { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? RejectionReason { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Subscription? Subscription { get; set; }
    public User? ApprovedByUser { get; set; }
}

public enum PaymentStatus
{
    Pending = 0,
    Success = 1,
    Failed = 2,
    Cancelled = 3,
    PendingApproval = 4,
    Approved = 5,
    Rejected = 6
}

public enum PaymentGateway
{
    Paytm = 0,
    Razorpay = 1,
    UPI = 2
}