namespace Domain.Entities;

public class Notice
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Information, Request, Announcement, etc.
    public string Priority { get; set; } = string.Empty; // Low, Medium, High, Urgent
    public int PostedByUserId { get; set; }
    public string PostedByUserName { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation property
    public User? PostedByUser { get; set; }
    public ICollection<NoticeReply> Replies { get; set; } = new List<NoticeReply>();
}