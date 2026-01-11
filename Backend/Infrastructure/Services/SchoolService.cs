using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class SchoolService : ISchoolService
{
    private readonly ISchoolRepository _schoolRepository;

    public SchoolService(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public async Task<IEnumerable<SchoolDto>> GetAllSchoolsAsync()
    {
        var schools = await _schoolRepository.GetAllAsync();
        return schools.Select(MapToDto);
    }

    public async Task<SchoolDto?> GetSchoolByIdAsync(int id)
    {
        var school = await _schoolRepository.GetByIdAsync(id);
        return school != null ? MapToDto(school) : null;
    }

    public async Task<SchoolDto> CreateSchoolAsync(CreateSchoolDto dto)
    {
        var school = new School
        {
            SchoolName = dto.SchoolName,
            SchoolCode = dto.SchoolCode,
            District = dto.District,
            Block = dto.Block,
            Village = dto.Village,
            SchoolType = dto.SchoolType,
            ManagementType = dto.ManagementType,
            TotalStudents = dto.TotalStudents,
            TotalTeachers = dto.TotalTeachers,
            PrincipalName = dto.PrincipalName,
            ContactNumber = dto.ContactNumber,
            Email = dto.Email,
            EstablishedDate = dto.EstablishedDate
        };

        var createdSchool = await _schoolRepository.AddAsync(school);
        return MapToDto(createdSchool);
    }

    public async Task<SchoolDto> UpdateSchoolAsync(UpdateSchoolDto dto)
    {
        var existingSchool = await _schoolRepository.GetByIdAsync(dto.Id);
        if (existingSchool == null)
        {
            throw new InvalidOperationException("School not found");
        }

        existingSchool.SchoolName = dto.SchoolName;
        existingSchool.SchoolCode = dto.SchoolCode;
        existingSchool.District = dto.District;
        existingSchool.Block = dto.Block;
        existingSchool.Village = dto.Village;
        existingSchool.SchoolType = dto.SchoolType;
        existingSchool.ManagementType = dto.ManagementType;
        existingSchool.TotalStudents = dto.TotalStudents;
        existingSchool.TotalTeachers = dto.TotalTeachers;
        existingSchool.PrincipalName = dto.PrincipalName;
        existingSchool.ContactNumber = dto.ContactNumber;
        existingSchool.Email = dto.Email;
        existingSchool.IsActive = dto.IsActive;
        existingSchool.EstablishedDate = dto.EstablishedDate;

        var updatedSchool = await _schoolRepository.UpdateAsync(existingSchool);
        return MapToDto(updatedSchool);
    }

    public async Task<bool> DeleteSchoolAsync(int id)
    {
        return await _schoolRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<SchoolDto>> GetSchoolsByDistrictAsync(string district)
    {
        var schools = await _schoolRepository.GetByDistrictAsync(district);
        return schools.Select(MapToDto);
    }

    private static SchoolDto MapToDto(School school)
    {
        return new SchoolDto
        {
            Id = school.Id,
            SchoolName = school.SchoolName,
            SchoolCode = school.SchoolCode,
            District = school.District,
            Block = school.Block,
            Village = school.Village,
            SchoolType = school.SchoolType,
            ManagementType = school.ManagementType,
            TotalStudents = school.TotalStudents,
            TotalTeachers = school.TotalTeachers,
            PrincipalName = school.PrincipalName,
            ContactNumber = school.ContactNumber,
            Email = school.Email,
            IsActive = school.IsActive,
            EstablishedDate = school.EstablishedDate
        };
    }
}