using Domain.Entities;

namespace Domain.Interfaces;

public interface ITeacherDocumentRepository
{
    Task<IEnumerable<TeacherDocument>> GetAllAsync();
    Task<TeacherDocument?> GetByIdAsync(int id);
    Task<TeacherDocument> AddAsync(TeacherDocument document);
    Task<TeacherDocument> UpdateAsync(TeacherDocument document);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TeacherDocument>> GetByTeacherIdAsync(int teacherId);
    Task<IEnumerable<TeacherDocument>> GetByUserIdAsync(int userId);
    Task<IEnumerable<TeacherDocument>> GetByDocumentTypeAsync(string documentType);
    Task<(IEnumerable<TeacherDocument> Documents, int TotalCount)> SearchDocumentsAsync(
        int? teacherId,
        string? documentType,
        string? searchTerm,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize);
}
