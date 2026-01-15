namespace Domain.Entities;

public class TeacherDocument
{
    public int Id { get; set; }
    public int? TeacherId { get; set; } // Nullable - for admin-created teachers
    public int? UserId { get; set; } // Nullable - for user self-uploaded documents
    public Teacher? Teacher { get; set; }
    public User? User { get; set; }
    public string DocumentType { get; set; } = string.Empty; // Resume, Matric, Inter, Graduate, PG, Other
    public string CustomDocumentType { get; set; } = string.Empty; // For custom types
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string BlobUrl { get; set; } = string.Empty;
    public string BlobContainerName { get; set; } = string.Empty;
    public string BlobFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeInBytes { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public DateTime UploadedDate { get; set; }
    public int UploadedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
