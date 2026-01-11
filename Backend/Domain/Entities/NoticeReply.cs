namespace Domain.Entities;

public class NoticeReply
{
    public int Id { get; set; }
    public int NoticeId { get; set; }
    public string ReplyMessage { get; set; } = string.Empty;
    public int RepliedByUserId { get; set; }
    public string RepliedByUserName { get; set; } = string.Empty;
    public DateTime RepliedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation properties
    public Notice? Notice { get; set; }
    public User? RepliedByUser { get; set; }
}