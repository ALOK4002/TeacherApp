using Domain.Entities;

namespace Domain.Interfaces;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task<Teacher> AddAsync(Teacher teacher);
    Task<Teacher> UpdateAsync(Teacher teacher);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Teacher>> GetByDistrictAsync(string district);
    Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId);
}