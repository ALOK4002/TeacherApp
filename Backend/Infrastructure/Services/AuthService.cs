using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;
    private readonly JwtService _jwtService;

    public AuthService(IUserRepository userRepository, PasswordService passwordService, JwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userRepository.GetByUserNameOrEmailAsync(dto.UserNameOrEmail);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists");
        }

        var user = new User
        {
            UserName = dto.UserNameOrEmail,
            Email = dto.UserNameOrEmail,
            PasswordHash = _passwordService.HashPassword(dto.Password),
            CreatedDate = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByUserNameOrEmailAsync(dto.UserNameOrEmail);
        if (user == null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = _jwtService.GenerateToken(user.UserName, user.Id);

        return new AuthResponseDto
        {
            Token = token,
            UserName = user.UserName
        };
    }
}