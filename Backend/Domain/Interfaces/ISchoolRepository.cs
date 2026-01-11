using Domain.Entities;

namespace Domain.Interfaces;

public interface ISchoolRepository
{
    Task<IEnumerable<School>> GetAllAsync();
    Task<School?> GetByIdAsync(int id);
    Task<School> AddAsync(School school);
    Task<School> UpdateAsync(School school);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<School>> GetByDistrictAsync(string district);
}