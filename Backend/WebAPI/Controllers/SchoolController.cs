using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchoolController : ControllerBase
{
    private readonly ISchoolService _schoolService;
    private readonly ILogger<SchoolController> _logger;

    public SchoolController(ISchoolService schoolService, ILogger<SchoolController> logger)
    {
        _schoolService = schoolService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSchools()
    {
        _logger.LogInformation("Entering GetAllSchools");
        try
        {
            var schools = await _schoolService.GetAllSchoolsAsync();
            _logger.LogInformation("Exiting GetAllSchools with count: {Count}", ((IEnumerable<object>?)schools)?.Count() ?? 0);
            return Ok(schools);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllSchools error");
            return StatusCode(500, new { message = "An error occurred while retrieving schools" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSchoolById(int id)
    {
        _logger.LogInformation("Entering GetSchoolById for Id: {Id}", id);
        try
        {
            var school = await _schoolService.GetSchoolByIdAsync(id);
            if (school == null)
            {
                _logger.LogWarning("GetSchoolById not found for Id: {Id}", id);
                return NotFound(new { message = "School not found" });
            }
            _logger.LogInformation("Exiting GetSchoolById successfully for Id: {Id}", id);
            return Ok(school);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetSchoolById error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving school" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolDto dto)
    {
        _logger.LogInformation("Entering CreateSchool with Name: {Name}", dto.SchoolName);
        try
        {
            var validator = new CreateSchoolValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("CreateSchool validation failed for Name: {Name}", dto.SchoolName);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var school = await _schoolService.CreateSchoolAsync(dto);
            _logger.LogInformation("Exiting CreateSchool successfully with Id: {Id}", school.Id);
            return CreatedAtAction(nameof(GetSchoolById), new { id = school.Id }, school);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateSchool error for Name: {Name}", dto.SchoolName);
            return StatusCode(500, new { message = "An error occurred while creating school" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchool(int id, [FromBody] UpdateSchoolDto dto)
    {
        _logger.LogInformation("Entering UpdateSchool for Id: {Id}", id);
        try
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("UpdateSchool ID mismatch: PathId={PathId}, BodyId={BodyId}", id, dto.Id);
                return BadRequest(new { message = "ID mismatch" });
            }

            var validator = new UpdateSchoolValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("UpdateSchool validation failed for Id: {Id}", id);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var school = await _schoolService.UpdateSchoolAsync(dto);
            _logger.LogInformation("Exiting UpdateSchool successfully for Id: {Id}", id);
            return Ok(school);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "UpdateSchool not found for Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateSchool error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the school" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchool(int id)
    {
        _logger.LogInformation("Entering DeleteSchool for Id: {Id}", id);
        try
        {
            var result = await _schoolService.DeleteSchoolAsync(id);
            if (!result)
            {
                _logger.LogWarning("DeleteSchool not found for Id: {Id}", id);
                return NotFound(new { message = "School not found" });
            }
            _logger.LogInformation("Exiting DeleteSchool successfully for Id: {Id}", id);
            return Ok(new { message = "School deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteSchool error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the school" });
        }
    }

    [HttpGet("district/{district}")]
    public async Task<IActionResult> GetSchoolsByDistrict(string district)
    {
        _logger.LogInformation("Entering GetSchoolsByDistrict for District: {District}", district);
        try
        {
            var schools = await _schoolService.GetSchoolsByDistrictAsync(district);
            _logger.LogInformation("Exiting GetSchoolsByDistrict with count: {Count}", ((IEnumerable<object>?)schools)?.Count() ?? 0);
            return Ok(schools);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetSchoolsByDistrict error for District: {District}", district);
            return StatusCode(500, new { message = "An error occurred while retrieving schools by district" });
        }
    }
}