using Application.DTOs;

namespace Application.Interfaces;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
    Task<TeacherDto?> GetTeacherByIdAsync(int id);
    Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto dto);
    Task<TeacherDto> UpdateTeacherAsync(UpdateTeacherDto dto);
    Task<bool> DeleteTeacherAsync(int id);
    Task<IEnumerable<TeacherDto>> GetTeachersByDistrictAsync(string district);
    Task<IEnumerable<TeacherDto>> GetTeachersBySchoolIdAsync(int schoolId);
    Task<PagedResult<TeacherReportDto>> GetTeacherReportAsync(TeacherReportSearchRequest request);
}