using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<TeacherRepository> _logger;

    public TeacherRepository(AppDbContext context, ILogger<TeacherRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync()
    {
        _logger.LogInformation("Entering GetAllAsync");
        var query = _context.Teachers
            .Include(t => t.School)
            .Where(t => t.IsActive)
            .OrderBy(t => t.TeacherName);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetAllAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetByIdAsync with Id: {Id}", id);
        var query = _context.Teachers
            .Include(t => t.School)
            .Where(t => t.Id == id && t.IsActive);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<Teacher> AddAsync(Teacher teacher)
    {
        _logger.LogInformation("Entering AddAsync for Name: {Name}", teacher.TeacherName);
        teacher.CreatedDate = DateTime.UtcNow;
        teacher.UpdatedDate = DateTime.UtcNow;
        teacher.IsActive = true;

        await _context.Teachers.AddAsync(teacher);
        _logger.LogInformation("SQL SaveChanges for AddAsync");
        await _context.SaveChangesAsync();
        
        // Load school information
        await _context.Entry(teacher)
            .Reference(t => t.School)
            .LoadAsync();
        _logger.LogInformation("Exiting AddAsync with Id: {Id}", teacher.Id);
        return teacher;
    }

    public async Task<Teacher> UpdateAsync(Teacher teacher)
    {
        _logger.LogInformation("Entering UpdateAsync for Id: {Id}", teacher.Id);
        teacher.UpdatedDate = DateTime.UtcNow;
        
        _context.Teachers.Update(teacher);
        _logger.LogInformation("SQL SaveChanges for UpdateAsync");
        await _context.SaveChangesAsync();
        
        // Load school information
        await _context.Entry(teacher)
            .Reference(t => t.School)
            .LoadAsync();
        _logger.LogInformation("Exiting UpdateAsync for Id: {Id}", teacher.Id);
        return teacher;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Entering DeleteAsync for Id: {Id}", id);
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            _logger.LogWarning("DeleteAsync: Teacher not found for Id: {Id}", id);
            return false;
        }

        teacher.IsActive = false;
        teacher.UpdatedDate = DateTime.UtcNow;
        
        _logger.LogInformation("SQL SaveChanges for DeleteAsync (soft delete) Id: {Id}", id);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting DeleteAsync successfully for Id: {Id}", id);
        return true;
    }

    public async Task<IEnumerable<Teacher>> GetByDistrictAsync(string district)
    {
        _logger.LogInformation("Entering GetByDistrictAsync for District: {District}", district);
        var query = _context.Teachers
            .Include(t => t.School)
            .Where(t => t.District.ToLower() == district.ToLower() && t.IsActive)
            .OrderBy(t => t.TeacherName);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetByDistrictAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId)
    {
        _logger.LogInformation("Entering GetBySchoolIdAsync for SchoolId: {SchoolId}", schoolId);
        var query = _context.Teachers
            .Include(t => t.School)
            .Where(t => t.SchoolId == schoolId && t.IsActive)
            .OrderBy(t => t.TeacherName);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetBySchoolIdAsync with count: {Count}", result.Count);
        return result;
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
        _logger.LogInformation("Entering GetTeachersForReportAsync with filters: SearchTerm={SearchTerm}, TeacherName={TeacherName}, SchoolName={SchoolName}, District={District}, Pincode={Pincode}, ContactNumber={ContactNumber}, Page={Page}, PageSize={PageSize}, SortBy={SortBy}, SortDirection={SortDirection}", 
            searchTerm, teacherName, schoolName, district, pincode, contactNumber, page, pageSize, sortBy, sortDirection);
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

        _logger.LogInformation("Exiting GetTeachersForReportAsync with TotalCount: {TotalCount}, ReturnedCount: {ReturnedCount}", totalCount, teachers.Count);
        return (teachers, totalCount);
    }
}