namespace Domain.Entities;

public class School
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string SchoolCode { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string SchoolType { get; set; } = string.Empty; // Primary, Middle, High, Senior Secondary
    public string ManagementType { get; set; } = string.Empty; // Government, Aided, Private
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public string PrincipalName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime EstablishedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}