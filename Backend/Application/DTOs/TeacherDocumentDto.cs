namespace Application.DTOs;

public class TeacherDocumentDto
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string CustomDocumentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string BlobUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeInBytes { get; set; }
    public string FileSizeFormatted { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public DateTime UploadedDate { get; set; }
    public int UploadedByUserId { get; set; }
    public bool IsActive { get; set; }
}

public class UploadTeacherDocumentDto
{
    public int TeacherId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string CustomDocumentType { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}

public class SendDocumentEmailDto
{
    public int DocumentId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class DocumentSearchRequest
{
    public int? TeacherId { get; set; }
    public string? DocumentType { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
