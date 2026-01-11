using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchoolController : ControllerBase
{
    private readonly ISchoolService _schoolService;

    public SchoolController(ISchoolService schoolService)
    {
        _schoolService = schoolService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSchools()
    {
        try
        {
            var schools = await _schoolService.GetAllSchoolsAsync();
            return Ok(schools);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving schools" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSchoolById(int id)
    {
        try
        {
            var school = await _schoolService.GetSchoolByIdAsync(id);
            if (school == null)
            {
                return NotFound(new { message = "School not found" });
            }
            return Ok(school);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the school" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolDto dto)
    {
        try
        {
            var validator = new CreateSchoolValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var school = await _schoolService.CreateSchoolAsync(dto);
            return CreatedAtAction(nameof(GetSchoolById), new { id = school.Id }, school);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the school" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchool(int id, [FromBody] UpdateSchoolDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var validator = new UpdateSchoolValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var school = await _schoolService.UpdateSchoolAsync(dto);
            return Ok(school);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the school" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchool(int id)
    {
        try
        {
            var result = await _schoolService.DeleteSchoolAsync(id);
            if (!result)
            {
                return NotFound(new { message = "School not found" });
            }
            return Ok(new { message = "School deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the school" });
        }
    }

    [HttpGet("district/{district}")]
    public async Task<IActionResult> GetSchoolsByDistrict(string district)
    {
        try
        {
            var schools = await _schoolService.GetSchoolsByDistrictAsync(district);
            return Ok(schools);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving schools by district" });
        }
    }
}