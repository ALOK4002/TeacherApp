namespace Application.DTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string ClassTeaching { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTeacherDto
{
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
}

public class UpdateTeacherDto
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
    public bool IsActive { get; set; }
}