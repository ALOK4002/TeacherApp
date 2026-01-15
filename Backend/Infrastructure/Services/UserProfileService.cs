using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _profileRepository;

    public UserProfileService(IUserProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<UserProfileDto?> GetMyProfileAsync(int userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        return profile != null ? MapToDto(profile) : null;
    }

    public async Task<UserProfileDto?> GetProfileByIdAsync(int id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        return profile != null ? MapToDto(profile) : null;
    }

    public async Task<UserProfileDto> CreateProfileAsync(int userId, CreateUserProfileDto dto)
    {
        // Check if profile already exists
        var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
        if (existingProfile != null)
        {
            throw new InvalidOperationException("Profile already exists for this user");
        }

        var profile = new UserProfile
        {
            UserId = userId,
            TeacherName = dto.TeacherName,
            Address = dto.Address,
            District = dto.District,
            Pincode = dto.Pincode,
            SchoolId = dto.SchoolId,
            ClassTeaching = dto.ClassTeaching,
            Subject = dto.Subject,
            Qualification = dto.Qualification,
            ContactNumber = dto.ContactNumber,
            Email = dto.Email,
            DateOfJoining = dto.DateOfJoining,
            IsActive = true
        };

        var createdProfile = await _profileRepository.AddAsync(profile);
        return MapToDto(createdProfile);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateUserProfileDto dto)
    {
        var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
        if (existingProfile == null)
        {
            throw new InvalidOperationException("Profile not found");
        }

        // Ensure user can only update their own profile
        if (existingProfile.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only update your own profile");
        }

        existingProfile.TeacherName = dto.TeacherName;
        existingProfile.Address = dto.Address;
        existingProfile.District = dto.District;
        existingProfile.Pincode = dto.Pincode;
        existingProfile.SchoolId = dto.SchoolId;
        existingProfile.ClassTeaching = dto.ClassTeaching;
        existingProfile.Subject = dto.Subject;
        existingProfile.Qualification = dto.Qualification;
        existingProfile.ContactNumber = dto.ContactNumber;
        existingProfile.Email = dto.Email;
        existingProfile.DateOfJoining = dto.DateOfJoining;
        existingProfile.IsActive = dto.IsActive;

        var updatedProfile = await _profileRepository.UpdateAsync(existingProfile);
        return MapToDto(updatedProfile);
    }

    public async Task<bool> HasProfileAsync(int userId)
    {
        return await _profileRepository.HasProfileAsync(userId);
    }

    private static UserProfileDto MapToDto(UserProfile profile)
    {
        return new UserProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            UserName = profile.User?.UserName ?? "",
            TeacherName = profile.TeacherName,
            Address = profile.Address,
            District = profile.District,
            Pincode = profile.Pincode,
            SchoolId = profile.SchoolId,
            SchoolName = profile.School?.SchoolName ?? "",
            ClassTeaching = profile.ClassTeaching,
            Subject = profile.Subject,
            Qualification = profile.Qualification,
            ContactNumber = profile.ContactNumber,
            Email = profile.Email,
            DateOfJoining = profile.DateOfJoining,
            IsActive = profile.IsActive,
            CreatedDate = profile.CreatedDate
        };
    }
}
