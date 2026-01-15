namespace Application.DTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string ClassTeaching { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTeacherDto
{
    public string TeacherName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string ClassTeaching { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
}

public class UpdateTeacherDto
{
    public int Id { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string ClassTeaching { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public bool IsActive { get; set; }
}

public class TeacherReportDto
{
    public int Id { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ClassTeaching { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public bool IsActive { get; set; }
}

public class TeacherReportSearchRequest
{
    public string? SearchTerm { get; set; }
    public string? TeacherName { get; set; }
    public string? SchoolName { get; set; }
    public string? District { get; set; }
    public string? Pincode { get; set; }
    public string? ContactNumber { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "TeacherName";
    public string? SortDirection { get; set; } = "asc";
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}