namespace Application.DTOs;

public class RegisterRequestDto
{
    public string UserNameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}