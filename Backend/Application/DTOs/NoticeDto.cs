namespace Application.DTOs;

public class NoticeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int PostedByUserId { get; set; }
    public string PostedByUserName { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public bool IsActive { get; set; }
    public int ReplyCount { get; set; }
    public bool HasReplied { get; set; } // For current user
}

public class CreateNoticeDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
}

public class UpdateNoticeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class NoticeReplyDto
{
    public int Id { get; set; }
    public int NoticeId { get; set; }
    public string ReplyMessage { get; set; } = string.Empty;
    public int RepliedByUserId { get; set; }
    public string RepliedByUserName { get; set; } = string.Empty;
    public DateTime RepliedDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateNoticeReplyDto
{
    public int NoticeId { get; set; }
    public string ReplyMessage { get; set; } = string.Empty;
}

public class NoticeWithRepliesDto
{
    public NoticeDto Notice { get; set; } = new();
    public List<NoticeReplyDto> Replies { get; set; } = new();
}