using System.Text.Json.Serialization;

namespace Application.DTOs;

public class RegisterDto
{
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    
    [JsonPropertyName("role")]
    public string Role { get; set; } = "Teacher"; // Admin or Teacher
}

public class LoginDto
{
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
    
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;
    
    [JsonPropertyName("isApproved")]
    public bool IsApproved { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}

public class UserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;
    
    [JsonPropertyName("isApproved")]
    public bool IsApproved { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("approvedByUserId")]
    public int? ApprovedByUserId { get; set; }
    
    [JsonPropertyName("approvedDate")]
    public DateTime? ApprovedDate { get; set; }
    
    [JsonPropertyName("rejectionReason")]
    public string? RejectionReason { get; set; }
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
}

public class ApproveUserDto
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("isApproved")]
    public bool IsApproved { get; set; }
    
    [JsonPropertyName("rejectionReason")]
    public string? RejectionReason { get; set; }
}

public class PendingUsersResponse
{
    [JsonPropertyName("pendingUsers")]
    public List<UserDto> PendingUsers { get; set; } = new();
    
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}
