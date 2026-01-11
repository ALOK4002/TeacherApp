using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SchoolRepository : ISchoolRepository
{
    private readonly AppDbContext _context;

    public SchoolRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<School>> GetAllAsync()
    {
        return await _context.Schools
            .Where(s => s.IsActive)
            .OrderBy(s => s.District)
            .ThenBy(s => s.SchoolName)
            .ToListAsync();
    }

    public async Task<School?> GetByIdAsync(int id)
    {
        return await _context.Schools
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<School> AddAsync(School school)
    {
        school.CreatedDate = DateTime.UtcNow;
        school.UpdatedDate = DateTime.UtcNow;
        school.IsActive = true;

        await _context.Schools.AddAsync(school);
        await _context.SaveChangesAsync();
        return school;
    }

    public async Task<School> UpdateAsync(School school)
    {
        school.UpdatedDate = DateTime.UtcNow;
        
        _context.Schools.Update(school);
        await _context.SaveChangesAsync();
        return school;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var school = await _context.Schools.FindAsync(id);
        if (school == null) return false;

        school.IsActive = false;
        school.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<School>> GetByDistrictAsync(string district)
    {
        return await _context.Schools
            .Where(s => s.District.ToLower() == district.ToLower() && s.IsActive)
            .OrderBy(s => s.SchoolName)
            .ToListAsync();
    }
}