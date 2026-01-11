using Application.DTOs;

namespace Application.Interfaces;

public interface ISchoolService
{
    Task<IEnumerable<SchoolDto>> GetAllSchoolsAsync();
    Task<SchoolDto?> GetSchoolByIdAsync(int id);
    Task<SchoolDto> CreateSchoolAsync(CreateSchoolDto dto);
    Task<SchoolDto> UpdateSchoolAsync(UpdateSchoolDto dto);
    Task<bool> DeleteSchoolAsync(int id);
    Task<IEnumerable<SchoolDto>> GetSchoolsByDistrictAsync(string district);
}