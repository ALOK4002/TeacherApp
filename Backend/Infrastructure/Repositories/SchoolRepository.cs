using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SchoolRepository : ISchoolRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<SchoolRepository> _logger;

    public SchoolRepository(AppDbContext context, ILogger<SchoolRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<School>> GetAllAsync()
    {
        _logger.LogInformation("Entering GetAllAsync");
        var query = _context.Schools
            .Where(s => s.IsActive)
            .OrderBy(s => s.District)
            .ThenBy(s => s.SchoolName);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetAllAsync with count: {Count}", result.Count);
        return result;
    }

    public async Task<School?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Entering GetByIdAsync with Id: {Id}", id);
        var query = _context.Schools
            .Where(s => s.Id == id && s.IsActive);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.FirstOrDefaultAsync();
        _logger.LogInformation("Exiting GetByIdAsync; found: {Found}", result != null);
        return result;
    }

    public async Task<School> AddAsync(School school)
    {
        _logger.LogInformation("Entering AddAsync for Name: {Name}", school.SchoolName);
        school.CreatedDate = DateTime.UtcNow;
        school.UpdatedDate = DateTime.UtcNow;
        school.IsActive = true;

        await _context.Schools.AddAsync(school);
        _logger.LogInformation("SQL SaveChanges for AddAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting AddAsync with Id: {Id}", school.Id);
        return school;
    }

    public async Task<School> UpdateAsync(School school)
    {
        _logger.LogInformation("Entering UpdateAsync for Id: {Id}", school.Id);
        school.UpdatedDate = DateTime.UtcNow;
        
        _context.Schools.Update(school);
        _logger.LogInformation("SQL SaveChanges for UpdateAsync");
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting UpdateAsync for Id: {Id}", school.Id);
        return school;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Entering DeleteAsync for Id: {Id}", id);
        var school = await _context.Schools.FindAsync(id);
        if (school == null)
        {
            _logger.LogWarning("DeleteAsync: School not found for Id: {Id}", id);
            return false;
        }

        school.IsActive = false;
        school.UpdatedDate = DateTime.UtcNow;
        
        _logger.LogInformation("SQL SaveChanges for DeleteAsync (soft delete) Id: {Id}", id);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exiting DeleteAsync successfully for Id: {Id}", id);
        return true;
    }

    public async Task<IEnumerable<School>> GetByDistrictAsync(string district)
    {
        _logger.LogInformation("Entering GetByDistrictAsync for District: {District}", district);
        var query = _context.Schools
            .Where(s => s.District.ToLower() == district.ToLower() && s.IsActive)
            .OrderBy(s => s.SchoolName);
        _logger.LogInformation("SQL Query: {Query}", query.ToQueryString());
        var result = await query.ToListAsync();
        _logger.LogInformation("Exiting GetByDistrictAsync with count: {Count}", result.Count);
        return result;
    }
}