using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;

    public TeacherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync()
    {
        return await _context.Teachers
            .Include(t => t.School)
            .Where(t => t.IsActive)
            .OrderBy(t => t.TeacherName)
            .ToListAsync();
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await _context.Teachers
            .Include(t => t.School)
            .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
    }

    public async Task<Teacher> AddAsync(Teacher teacher)
    {
        teacher.CreatedDate = DateTime.UtcNow;
        teacher.UpdatedDate = DateTime.UtcNow;
        teacher.IsActive = true;

        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
        
        // Load the school information
        await _context.Entry(teacher)
            .Reference(t => t.School)
            .LoadAsync();
            
        return teacher;
    }

    public async Task<Teacher> UpdateAsync(Teacher teacher)
    {
        teacher.UpdatedDate = DateTime.UtcNow;
        
        _context.Teachers.Update(teacher);
        await _context.SaveChangesAsync();
        
        // Load the school information
        await _context.Entry(teacher)
            .Reference(t => t.School)
            .LoadAsync();
            
        return teacher;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return false;

        teacher.IsActive = false;
        teacher.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Teacher>> GetByDistrictAsync(string district)
    {
        return await _context.Teachers
            .Include(t => t.School)
            .Where(t => t.District.ToLower() == district.ToLower() && t.IsActive)
            .OrderBy(t => t.TeacherName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId)
    {
        return await _context.Teachers
            .Include(t => t.School)
            .Where(t => t.SchoolId == schoolId && t.IsActive)
            .OrderBy(t => t.TeacherName)
            .ToListAsync();
    }
}