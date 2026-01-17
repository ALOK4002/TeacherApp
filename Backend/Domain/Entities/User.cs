namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Teacher"; // Admin or Teacher
    public bool IsApproved { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int? ApprovedByUserId { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public Subscription? Subscription { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<UserActivity> Activities { get; set; } = new List<UserActivity>();
}