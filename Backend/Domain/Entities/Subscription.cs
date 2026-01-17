namespace Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public SubscriptionTier Tier { get; set; } = SubscriptionTier.Free;
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int DocumentUploadLimit { get; set; } = 3; // Free: 3, Premium: 10
    public long FileSizeLimitInBytes { get; set; } = 512000; // Free: 500KB, Premium: 1MB
    public int DocumentsUploaded { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public enum SubscriptionTier
{
    Free = 0,
    Premium = 1
}

public enum SubscriptionStatus
{
    Active = 0,
    Expired = 1,
    Cancelled = 2,
    PendingApproval = 3
}