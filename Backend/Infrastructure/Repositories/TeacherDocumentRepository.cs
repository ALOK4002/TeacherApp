using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TeacherDocumentRepository : ITeacherDocumentRepository
{
    private readonly AppDbContext _context;

    public TeacherDocumentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TeacherDocument>> GetAllAsync()
    {
        return await _context.TeacherDocuments
            .Include(d => d.Teacher)
            .Where(d => d.IsActive)
            .OrderByDescending(d => d.UploadedDate)
            .ToListAsync();
    }

    public async Task<TeacherDocument?> GetByIdAsync(int id)
    {
        return await _context.TeacherDocuments
            .Include(d => d.Teacher)
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
    }

    public async Task<TeacherDocument> AddAsync(TeacherDocument document)
    {
        document.CreatedDate = DateTime.UtcNow;
        document.UpdatedDate = DateTime.UtcNow;
        document.IsActive = true;

        await _context.TeacherDocuments.AddAsync(document);
        await _context.SaveChangesAsync();
        
        await _context.Entry(document)
            .Reference(d => d.Teacher)
            .LoadAsync();
            
        return document;
    }

    public async Task<TeacherDocument> UpdateAsync(TeacherDocument document)
    {
        document.UpdatedDate = DateTime.UtcNow;
        
        _context.TeacherDocuments.Update(document);
        await _context.SaveChangesAsync();
        
        await _context.Entry(document)
            .Reference(d => d.Teacher)
            .LoadAsync();
            
        return document;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var document = await _context.TeacherDocuments.FindAsync(id);
        if (document == null) return false;

        document.IsActive = false;
        document.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TeacherDocument>> GetByTeacherIdAsync(int teacherId)
    {
        return await _context.TeacherDocuments
            .Include(d => d.Teacher)
            .Where(d => d.TeacherId == teacherId && d.IsActive)
            .OrderByDescending(d => d.UploadedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TeacherDocument>> GetByUserIdAsync(int userId)
    {
        return await _context.TeacherDocuments
            .Include(d => d.User)
            .Where(d => d.UserId == userId && d.IsActive)
            .OrderByDescending(d => d.UploadedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TeacherDocument>> GetByDocumentTypeAsync(string documentType)
    {
        return await _context.TeacherDocuments
            .Include(d => d.Teacher)
            .Where(d => d.DocumentType.ToLower() == documentType.ToLower() && d.IsActive)
            .OrderByDescending(d => d.UploadedDate)
            .ToListAsync();
    }

    public async Task<(IEnumerable<TeacherDocument> Documents, int TotalCount)> SearchDocumentsAsync(
        int? teacherId,
        string? documentType,
        string? searchTerm,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize)
    {
        var query = _context.TeacherDocuments
            .Include(d => d.Teacher)
            .Where(d => d.IsActive)
            .AsQueryable();

        if (teacherId.HasValue)
        {
            query = query.Where(d => d.TeacherId == teacherId.Value);
        }

        if (!string.IsNullOrWhiteSpace(documentType))
        {
            query = query.Where(d => d.DocumentType.ToLower() == documentType.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(d =>
                d.FileName.ToLower().Contains(search) ||
                d.OriginalFileName.ToLower().Contains(search) ||
                d.Teacher.TeacherName.ToLower().Contains(search) ||
                d.Remarks.ToLower().Contains(search));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(d => d.UploadedDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(d => d.UploadedDate <= toDate.Value);
        }

        var totalCount = await query.CountAsync();

        var documents = await query
            .OrderByDescending(d => d.UploadedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (documents, totalCount);
    }
}
