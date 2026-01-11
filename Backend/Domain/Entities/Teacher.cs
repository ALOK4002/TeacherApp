namespace Domain.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string ClassTeaching { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // Navigation property
    public School? School { get; set; }
}