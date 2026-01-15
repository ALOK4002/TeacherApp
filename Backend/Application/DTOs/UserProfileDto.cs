using System.Text.Json.Serialization;

namespace Application.DTOs;

public class UserProfileDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;
    
    [JsonPropertyName("teacherName")]
    public string TeacherName { get; set; } = string.Empty;
    
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    
    [JsonPropertyName("district")]
    public string District { get; set; } = string.Empty;
    
    [JsonPropertyName("pincode")]
    public string Pincode { get; set; } = string.Empty;
    
    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }
    
    [JsonPropertyName("schoolName")]
    public string SchoolName { get; set; } = string.Empty;
    
    [JsonPropertyName("classTeaching")]
    public string ClassTeaching { get; set; } = string.Empty;
    
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [JsonPropertyName("qualification")]
    public string Qualification { get; set; } = string.Empty;
    
    [JsonPropertyName("contactNumber")]
    public string ContactNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("dateOfJoining")]
    public DateTime DateOfJoining { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
}

public class CreateUserProfileDto
{
    [JsonPropertyName("teacherName")]
    public string TeacherName { get; set; } = string.Empty;
    
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    
    [JsonPropertyName("district")]
    public string District { get; set; } = string.Empty;
    
    [JsonPropertyName("pincode")]
    public string Pincode { get; set; } = string.Empty;
    
    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }
    
    [JsonPropertyName("classTeaching")]
    public string ClassTeaching { get; set; } = string.Empty;
    
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [JsonPropertyName("qualification")]
    public string Qualification { get; set; } = string.Empty;
    
    [JsonPropertyName("contactNumber")]
    public string ContactNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("dateOfJoining")]
    public DateTime DateOfJoining { get; set; }
}

public class UpdateUserProfileDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("teacherName")]
    public string TeacherName { get; set; } = string.Empty;
    
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;
    
    [JsonPropertyName("district")]
    public string District { get; set; } = string.Empty;
    
    [JsonPropertyName("pincode")]
    public string Pincode { get; set; } = string.Empty;
    
    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }
    
    [JsonPropertyName("classTeaching")]
    public string ClassTeaching { get; set; } = string.Empty;
    
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [JsonPropertyName("qualification")]
    public string Qualification { get; set; } = string.Empty;
    
    [JsonPropertyName("contactNumber")]
    public string ContactNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("dateOfJoining")]
    public DateTime DateOfJoining { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}
