using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using System.Text.Json.Serialization;

namespace Application.Models;

// Teacher Search Document
public class TeacherSearchDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string Name { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true)]
    public string Email { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true)]
    public string Address { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string District { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true)]
    public string Pincode { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string SchoolName { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Class { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public DateTime CreatedDate { get; set; }

    [SimpleField(IsFilterable = true)]
    public int SchoolId { get; set; }
}

// School Search Document
public class SchoolSearchDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string Name { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string District { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true)]
    public string Address { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true)]
    public string Pincode { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Type { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true)]
    public bool IsActive { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public DateTime EstablishedDate { get; set; }

    [SimpleField(IsFilterable = true)]
    public int TeacherCount { get; set; }
}

// Notice Search Document
public class NoticeSearchDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string Title { get; set; } = string.Empty;

    [SearchableField]
    public string Message { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Category { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string Priority { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true)]
    public string PostedByUserName { get; set; } = string.Empty;

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public DateTime PostedDate { get; set; }

    [SimpleField(IsFilterable = true)]
    public bool IsActive { get; set; }

    [SimpleField(IsFilterable = true)]
    public int ReplyCount { get; set; }
}

// Search Request Models
public class SearchRequest
{
    public string Query { get; set; } = "*";
    public string[]? Filters { get; set; }
    public string[]? Facets { get; set; }
    public string? OrderBy { get; set; }
    public int Skip { get; set; } = 0;
    public int Top { get; set; } = 50;
    public bool IncludeTotalCount { get; set; } = true;
}

// Search Response Models
public class SearchResponse<T>
{
    public IList<T> Results { get; set; } = new List<T>();
    public long? TotalCount { get; set; }
    public IDictionary<string, IList<FacetResult>>? Facets { get; set; }
    public string? ContinuationToken { get; set; }
}

public class FacetResult
{
    public object? Value { get; set; }
    public long? Count { get; set; }
}

// Unified Search Request
public class UnifiedSearchRequest
{
    public string Query { get; set; } = "*";
    public string[]? SearchTypes { get; set; } // "teachers", "schools", "notices"
    public string[]? Filters { get; set; }
    public int Skip { get; set; } = 0;
    public int Top { get; set; } = 20;
}

// Unified Search Result
public class UnifiedSearchResult
{
    public string Type { get; set; } = string.Empty; // "teacher", "school", "notice"
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public double Score { get; set; }
    public object? Data { get; set; }
}

public class UnifiedSearchResponse
{
    public IList<UnifiedSearchResult> Results { get; set; } = new List<UnifiedSearchResult>();
    public long TotalCount { get; set; }
    public Dictionary<string, long> TypeCounts { get; set; } = new();
}