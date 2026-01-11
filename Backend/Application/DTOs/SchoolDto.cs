namespace Application.DTOs;

public class SchoolDto
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string SchoolCode { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string SchoolType { get; set; } = string.Empty;
    public string ManagementType { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public string PrincipalName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime EstablishedDate { get; set; }
}

public class CreateSchoolDto
{
    public string SchoolName { get; set; } = string.Empty;
    public string SchoolCode { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string SchoolType { get; set; } = string.Empty;
    public string ManagementType { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public string PrincipalName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EstablishedDate { get; set; }
}

public class UpdateSchoolDto
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string SchoolCode { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string SchoolType { get; set; } = string.Empty;
    public string ManagementType { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public string PrincipalName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime EstablishedDate { get; set; }
}