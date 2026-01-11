using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        try
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving teachers" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        try
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
            {
                return NotFound(new { message = "Teacher not found" });
            }
            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the teacher" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto dto)
    {
        try
        {
            var validator = new CreateTeacherValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var teacher = await _teacherService.CreateTeacherAsync(dto);
            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.Id }, teacher);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the teacher" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, [FromBody] UpdateTeacherDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var validator = new UpdateTeacherValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var teacher = await _teacherService.UpdateTeacherAsync(dto);
            return Ok(teacher);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the teacher" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        try
        {
            var result = await _teacherService.DeleteTeacherAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Teacher not found" });
            }
            return Ok(new { message = "Teacher deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the teacher" });
        }
    }

    [HttpGet("district/{district}")]
    public async Task<IActionResult> GetTeachersByDistrict(string district)
    {
        try
        {
            var teachers = await _teacherService.GetTeachersByDistrictAsync(district);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving teachers by district" });
        }
    }

    [HttpGet("school/{schoolId}")]
    public async Task<IActionResult> GetTeachersBySchool(int schoolId)
    {
        try
        {
            var teachers = await _teacherService.GetTeachersBySchoolIdAsync(schoolId);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving teachers by school" });
        }
    }
}