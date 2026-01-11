using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ISearchService searchService, ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    /// <summary>
    /// Unified search across all entities (teachers, schools, notices)
    /// </summary>
    [HttpPost("unified")]
    public async Task<IActionResult> UnifiedSearch([FromBody] UnifiedSearchRequest request)
    {
        try
        {
            var results = await _searchService.UnifiedSearchAsync(request);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing unified search");
            return StatusCode(500, new { message = "An error occurred while searching" });
        }
    }

    /// <summary>
    /// Search teachers
    /// </summary>
    [HttpPost("teachers")]
    public async Task<IActionResult> SearchTeachers([FromBody] SearchRequest request)
    {
        try
        {
            var results = await _searchService.SearchTeachersAsync(request);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching teachers");
            return StatusCode(500, new { message = "An error occurred while searching teachers" });
        }
    }

    /// <summary>
    /// Search schools
    /// </summary>
    [HttpPost("schools")]
    public async Task<IActionResult> SearchSchools([FromBody] SearchRequest request)
    {
        try
        {
            var results = await _searchService.SearchSchoolsAsync(request);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching schools");
            return StatusCode(500, new { message = "An error occurred while searching schools" });
        }
    }

    /// <summary>
    /// Search notices
    /// </summary>
    [HttpPost("notices")]
    public async Task<IActionResult> SearchNotices([FromBody] SearchRequest request)
    {
        try
        {
            var results = await _searchService.SearchNoticesAsync(request);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching notices");
            return StatusCode(500, new { message = "An error occurred while searching notices" });
        }
    }

    /// <summary>
    /// Get search suggestions for autocomplete
    /// </summary>
    [HttpGet("suggestions")]
    public async Task<IActionResult> GetSuggestions(
        [FromQuery] string query, 
        [FromQuery] string type = "teachers")
    {
        try
        {
            var indexName = type.ToLower() switch
            {
                "teachers" => "teachers-index",
                "schools" => "schools-index",
                "notices" => "notices-index",
                _ => "teachers-index"
            };

            var suggestions = await _searchService.GetSuggestionsAsync(indexName, query, "sg");
            return Ok(suggestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting search suggestions");
            return StatusCode(500, new { message = "An error occurred while getting suggestions" });
        }
    }

    /// <summary>
    /// Get autocomplete suggestions
    /// </summary>
    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete(
        [FromQuery] string query, 
        [FromQuery] string type = "teachers")
    {
        try
        {
            var indexName = type.ToLower() switch
            {
                "teachers" => "teachers-index",
                "schools" => "schools-index",
                "notices" => "notices-index",
                _ => "teachers-index"
            };

            var completions = await _searchService.AutocompleteAsync(indexName, query, "sg");
            return Ok(completions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting autocomplete suggestions");
            return StatusCode(500, new { message = "An error occurred while getting autocomplete suggestions" });
        }
    }

    /// <summary>
    /// Initialize search indexes (Admin only)
    /// </summary>
    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeIndexes()
    {
        try
        {
            await _searchService.InitializeIndexesAsync();
            return Ok(new { message = "Search indexes initialized successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing search indexes");
            return StatusCode(500, new { message = "An error occurred while initializing search indexes" });
        }
    }

    /// <summary>
    /// Sync all data to search indexes (Admin only)
    /// </summary>
    [HttpPost("sync")]
    public async Task<IActionResult> SyncData([FromQuery] string? type = null)
    {
        try
        {
            if (string.IsNullOrEmpty(type))
            {
                await _searchService.SyncAllDataAsync();
                return Ok(new { message = "All data synced successfully" });
            }

            switch (type.ToLower())
            {
                case "teachers":
                    await _searchService.SyncTeachersAsync();
                    return Ok(new { message = "Teachers data synced successfully" });
                case "schools":
                    await _searchService.SyncSchoolsAsync();
                    return Ok(new { message = "Schools data synced successfully" });
                case "notices":
                    await _searchService.SyncNoticesAsync();
                    return Ok(new { message = "Notices data synced successfully" });
                default:
                    return BadRequest(new { message = "Invalid sync type. Use 'teachers', 'schools', or 'notices'" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing data to search indexes");
            return StatusCode(500, new { message = "An error occurred while syncing data" });
        }
    }

    /// <summary>
    /// Get search statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetSearchStats()
    {
        try
        {
            var stats = new
            {
                TeachersIndexExists = await _searchService.IndexExistsAsync("teachers-index"),
                SchoolsIndexExists = await _searchService.IndexExistsAsync("schools-index"),
                NoticesIndexExists = await _searchService.IndexExistsAsync("notices-index"),
                Timestamp = DateTime.UtcNow
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting search statistics");
            return StatusCode(500, new { message = "An error occurred while getting search statistics" });
        }
    }
}