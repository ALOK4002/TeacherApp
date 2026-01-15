namespace Application.DTOs;

public class UserProfileDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
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
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateUserProfileDto
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
}

public class UpdateUserProfileDto
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
    public bool IsActive { get; set; }
}
