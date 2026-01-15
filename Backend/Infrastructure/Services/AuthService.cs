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

    public async Task RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userRepository.GetByUserNameOrEmailAsync(dto.UserName);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        var existingEmail = await _userRepository.GetByUserNameOrEmailAsync(dto.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = _passwordService.HashPassword(dto.Password),
            Role = dto.Role,
            IsApproved = false, // Requires admin approval
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUserNameOrEmailAsync(dto.UserName);
        if (user == null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Check if user is approved (except for admin)
        if (user.Role != "Admin" && !user.IsApproved)
        {
            throw new UnauthorizedAccessException("Your account is pending approval. Please wait for admin approval.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Your account has been deactivated.");
        }

        var token = _jwtService.GenerateToken(user.UserName, user.Id, user.Role);

        return new AuthResponseDto
        {
            Token = token,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            IsApproved = user.IsApproved,
            IsActive = user.IsActive
        };
    }

    public async Task<PendingUsersResponse> GetPendingUsersAsync()
    {
        var pendingUsers = await _userRepository.GetPendingUsersAsync();
        
        return new PendingUsersResponse
        {
            PendingUsers = pendingUsers.Select(MapToDto).ToList(),
            TotalCount = pendingUsers.Count
        };
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(MapToDto).ToList();
    }

    public async Task ApproveUserAsync(int userId, int approvedByUserId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.IsApproved = true;
        user.ApprovedByUserId = approvedByUserId;
        user.ApprovedDate = DateTime.UtcNow;
        user.RejectionReason = null;

        await _userRepository.UpdateAsync(user);
    }

    public async Task RejectUserAsync(int userId, string rejectionReason, int rejectedByUserId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.IsApproved = false;
        user.IsActive = false;
        user.RejectionReason = rejectionReason;
        user.ApprovedByUserId = rejectedByUserId;
        user.ApprovedDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            IsApproved = user.IsApproved,
            IsActive = user.IsActive,
            ApprovedByUserId = user.ApprovedByUserId,
            ApprovedDate = user.ApprovedDate,
            RejectionReason = user.RejectionReason,
            CreatedDate = user.CreatedDate
        };
    }
}