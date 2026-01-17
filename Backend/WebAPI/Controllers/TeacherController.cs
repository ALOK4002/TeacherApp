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
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger)
    {
        _teacherService = teacherService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        _logger.LogInformation("Entering GetAllTeachers");
        try
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            _logger.LogInformation("Exiting GetAllTeachers with count: {Count}", ((IEnumerable<object>?)teachers)?.Count() ?? 0);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllTeachers error");
            return StatusCode(500, new { message = "An error occurred while retrieving teachers" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        _logger.LogInformation("Entering GetTeacherById for Id: {Id}", id);
        try
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
            {
                _logger.LogWarning("GetTeacherById not found for Id: {Id}", id);
                return NotFound(new { message = "Teacher not found" });
            }
            _logger.LogInformation("Exiting GetTeacherById successfully for Id: {Id}", id);
            return Ok(teacher);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTeacherById error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving teacher" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto dto)
    {
        _logger.LogInformation("Entering CreateTeacher with Name: {Name}", dto.TeacherName);
        try
        {
            var validator = new CreateTeacherValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("CreateTeacher validation failed for Name: {Name}", dto.TeacherName);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var teacher = await _teacherService.CreateTeacherAsync(dto);
            _logger.LogInformation("Exiting CreateTeacher successfully with Id: {Id}", teacher.Id);
            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.Id }, teacher);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateTeacher error for Name: {Name}", dto.TeacherName);
            return StatusCode(500, new { message = "An error occurred while creating teacher" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] UpdateTeacherDto dto)
    {
        _logger.LogInformation("Entering UpdateTeacher for Id: {Id}", id);
        try
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("UpdateTeacher ID mismatch: PathId={PathId}, BodyId={BodyId}", id, dto.Id);
                return BadRequest(new { message = "ID mismatch" });
            }

            var validator = new UpdateTeacherValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("UpdateTeacher validation failed for Id: {Id}", id);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var teacher = await _teacherService.UpdateTeacherAsync(dto);
            _logger.LogInformation("Exiting UpdateTeacher successfully for Id: {Id}", id);
            return Ok(teacher);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "UpdateTeacher not found for Id: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateTeacher error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the teacher" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        _logger.LogInformation("Entering DeleteTeacher for Id: {Id}", id);
        try
        {
            var result = await _teacherService.DeleteTeacherAsync(id);
            if (!result)
            {
                _logger.LogWarning("DeleteTeacher not found for Id: {Id}", id);
                return NotFound(new { message = "Teacher not found" });
            }
            _logger.LogInformation("Exiting DeleteTeacher successfully for Id: {Id}", id);
            return Ok(new { message = "Teacher deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteTeacher error for Id: {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the teacher" });
        }
    }

    [HttpGet("district/{district}")]
    public async Task<IActionResult> GetTeachersByDistrict(string district)
    {
        _logger.LogInformation("Entering GetTeachersByDistrict for District: {District}", district);
        try
        {
            var teachers = await _teacherService.GetTeachersByDistrictAsync(district);
            _logger.LogInformation("Exiting GetTeachersByDistrict with count: {Count}", ((IEnumerable<object>?)teachers)?.Count() ?? 0);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTeachersByDistrict error for District: {District}", district);
            return StatusCode(500, new { message = "An error occurred while retrieving teachers by district" });
        }
    }

    [HttpGet("school/{schoolId}")]
    public async Task<IActionResult> GetTeachersBySchool(int schoolId)
    {
        _logger.LogInformation("Entering GetTeachersBySchool for SchoolId: {SchoolId}", schoolId);
        try
        {
            var teachers = await _teacherService.GetTeachersBySchoolIdAsync(schoolId);
            _logger.LogInformation("Exiting GetTeachersBySchool with count: {Count}", ((IEnumerable<object>?)teachers)?.Count() ?? 0);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTeachersBySchool error for SchoolId: {SchoolId}", schoolId);
            return StatusCode(500, new { message = "An error occurred while retrieving teachers by school" });
        }
    }

    [HttpPost("report")]
    public async Task<IActionResult> GetTeacherReport([FromBody] TeacherReportSearchRequest request)
    {
        _logger.LogInformation("Entering GetTeacherReport with Request: {@Request}", request);
        try
        {
            var result = await _teacherService.GetTeacherReportAsync(request);
            _logger.LogInformation("Exiting GetTeacherReport successfully");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTeacherReport error");
            return StatusCode(500, new { message = "An error occurred while retrieving the teacher report" });
        }
    }
}