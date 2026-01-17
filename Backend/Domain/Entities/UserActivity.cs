namespace Domain.Entities;

public class UserActivity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ActivityType ActivityType { get; set; }
    public string ActivityDescription { get; set; } = string.Empty;
    public string? EntityType { get; set; } // Document, Payment, Subscription
    public int? EntityId { get; set; }
    public string? Metadata { get; set; } // JSON for additional data
    public DateTime ActivityDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public User? User { get; set; }
}

public enum ActivityType
{
    DocumentUploaded = 0,
    DocumentDeleted = 1,
    DocumentViewed = 2,
    DocumentDownloaded = 3,
    DocumentEmailed = 4,
    PaymentInitiated = 5,
    PaymentCompleted = 6,
    PaymentFailed = 7,
    SubscriptionUpgraded = 8,
    SubscriptionExpired = 9,
    ProfileCreated = 10,
    ProfileUpdated = 11,
    Login = 12,
    Logout = 13
}