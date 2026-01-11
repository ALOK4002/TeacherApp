using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtilityController : ControllerBase
{
    private readonly IUtilityService _utilityService;

    public UtilityController(IUtilityService utilityService)
    {
        _utilityService = utilityService;
    }

    [HttpGet("districts")]
    public async Task<IActionResult> GetBiharDistricts()
    {
        try
        {
            var districts = await _utilityService.GetBiharDistrictsAsync();
            return Ok(districts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving districts" });
        }
    }

    [HttpGet("pincodes/{district}")]
    public async Task<IActionResult> GetPincodesByDistrict(string district)
    {
        try
        {
            var pincodes = await _utilityService.GetPincodesByDistrictAsync(district);
            return Ok(pincodes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving pincodes" });
        }
    }

    [HttpGet("district/{pincode}")]
    public async Task<IActionResult> GetDistrictByPincode(string pincode)
    {
        try
        {
            var district = await _utilityService.GetDistrictByPincodeAsync(pincode);
            if (district == null)
            {
                return NotFound(new { message = "District not found for the given pincode" });
            }
            return Ok(new { district });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving district" });
        }
    }
}