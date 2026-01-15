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

    public async Task<(IEnumerable<Teacher> Teachers, int TotalCount)> GetTeachersForReportAsync(
        string? searchTerm, 
        string? teacherName, 
        string? schoolName, 
        string? district, 
        string? pincode, 
        string? contactNumber,
        int page, 
        int pageSize, 
        string sortBy, 
        string sortDirection)
    {
        var query = _context.Teachers
            .Include(t => t.School)
            .Where(t => t.IsActive)
            .AsQueryable();

        // Apply search filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(t => 
                t.TeacherName.ToLower().Contains(search) ||
                t.School.SchoolName.ToLower().Contains(search) ||
                t.District.ToLower().Contains(search) ||
                t.Pincode.Contains(search) ||
                t.ContactNumber.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(teacherName))
        {
            query = query.Where(t => t.TeacherName.ToLower().Contains(teacherName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(schoolName))
        {
            query = query.Where(t => t.School.SchoolName.ToLower().Contains(schoolName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(district))
        {
            query = query.Where(t => t.District.ToLower().Contains(district.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(pincode))
        {
            query = query.Where(t => t.Pincode.Contains(pincode));
        }

        if (!string.IsNullOrWhiteSpace(contactNumber))
        {
            query = query.Where(t => t.ContactNumber.Contains(contactNumber));
        }

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "teachername" => sortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.TeacherName)
                : query.OrderBy(t => t.TeacherName),
            "schoolname" => sortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.School.SchoolName)
                : query.OrderBy(t => t.School.SchoolName),
            "district" => sortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.District)
                : query.OrderBy(t => t.District),
            "pincode" => sortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.Pincode)
                : query.OrderBy(t => t.Pincode),
            "contactnumber" => sortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.ContactNumber)
                : query.OrderBy(t => t.ContactNumber),
            _ => query.OrderBy(t => t.TeacherName)
        };

        var totalCount = await query.CountAsync();

        var teachers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (teachers, totalCount);
    }
}