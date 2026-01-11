using Application.DTOs;

namespace Application.Interfaces;

public interface IUtilityService
{
    Task<IEnumerable<DistrictDto>> GetBiharDistrictsAsync();
    Task<IEnumerable<PincodeDto>> GetPincodesByDistrictAsync(string district);
    Task<string?> GetDistrictByPincodeAsync(string pincode);
}