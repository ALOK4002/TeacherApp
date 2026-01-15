using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
    {
        var teachers = await _teacherRepository.GetAllAsync();
        return teachers.Select(MapToDto);
    }

    public async Task<TeacherDto?> GetTeacherByIdAsync(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        return teacher != null ? MapToDto(teacher) : null;
    }

    public async Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto dto)
    {
        var teacher = new Teacher
        {
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
            DateOfJoining = dto.DateOfJoining
        };

        var createdTeacher = await _teacherRepository.AddAsync(teacher);
        return MapToDto(createdTeacher);
    }

    public async Task<TeacherDto> UpdateTeacherAsync(UpdateTeacherDto dto)
    {
        var existingTeacher = await _teacherRepository.GetByIdAsync(dto.Id);
        if (existingTeacher == null)
        {
            throw new InvalidOperationException("Teacher not found");
        }

        existingTeacher.TeacherName = dto.TeacherName;
        existingTeacher.Address = dto.Address;
        existingTeacher.District = dto.District;
        existingTeacher.Pincode = dto.Pincode;
        existingTeacher.SchoolId = dto.SchoolId;
        existingTeacher.ClassTeaching = dto.ClassTeaching;
        existingTeacher.Subject = dto.Subject;
        existingTeacher.Qualification = dto.Qualification;
        existingTeacher.ContactNumber = dto.ContactNumber;
        existingTeacher.Email = dto.Email;
        existingTeacher.DateOfJoining = dto.DateOfJoining;
        existingTeacher.IsActive = dto.IsActive;

        var updatedTeacher = await _teacherRepository.UpdateAsync(existingTeacher);
        return MapToDto(updatedTeacher);
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        return await _teacherRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<TeacherDto>> GetTeachersByDistrictAsync(string district)
    {
        var teachers = await _teacherRepository.GetByDistrictAsync(district);
        return teachers.Select(MapToDto);
    }

    public async Task<IEnumerable<TeacherDto>> GetTeachersBySchoolIdAsync(int schoolId)
    {
        var teachers = await _teacherRepository.GetBySchoolIdAsync(schoolId);
        return teachers.Select(MapToDto);
    }

    private static TeacherDto MapToDto(Teacher teacher)
    {
        return new TeacherDto
        {
            Id = teacher.Id,
            TeacherName = teacher.TeacherName,
            Address = teacher.Address,
            District = teacher.District,
            Pincode = teacher.Pincode,
            SchoolId = teacher.SchoolId,
            SchoolName = teacher.School?.SchoolName ?? "",
            ClassTeaching = teacher.ClassTeaching,
            Subject = teacher.Subject,
            Qualification = teacher.Qualification,
            ContactNumber = teacher.ContactNumber,
            Email = teacher.Email,
            DateOfJoining = teacher.DateOfJoining,
            IsActive = teacher.IsActive
        };
    }

    public async Task<PagedResult<TeacherReportDto>> GetTeacherReportAsync(TeacherReportSearchRequest request)
    {
        var (teachers, totalCount) = await _teacherRepository.GetTeachersForReportAsync(
            request.SearchTerm,
            request.TeacherName,
            request.SchoolName,
            request.District,
            request.Pincode,
            request.ContactNumber,
            request.Page,
            request.PageSize,
            request.SortBy ?? "TeacherName",
            request.SortDirection ?? "asc"
        );

        var reportDtos = teachers.Select(MapToReportDto).ToList();

        return new PagedResult<TeacherReportDto>
        {
            Items = reportDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    private static TeacherReportDto MapToReportDto(Teacher teacher)
    {
        return new TeacherReportDto
        {
            Id = teacher.Id,
            TeacherName = teacher.TeacherName,
            SchoolName = teacher.School?.SchoolName ?? "",
            District = teacher.District,
            Pincode = teacher.Pincode,
            ContactNumber = teacher.ContactNumber,
            Email = teacher.Email,
            Address = teacher.Address,
            ClassTeaching = teacher.ClassTeaching,
            Subject = teacher.Subject,
            DateOfJoining = teacher.DateOfJoining,
            IsActive = teacher.IsActive
        };
    }
}