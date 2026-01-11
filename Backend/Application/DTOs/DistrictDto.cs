namespace Application.DTOs;

public class DistrictDto
{
    public string Name { get; set; } = string.Empty;
    public List<string> Pincodes { get; set; } = new();
}

public class PincodeDto
{
    public string Pincode { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
}