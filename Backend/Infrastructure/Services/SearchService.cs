using Application.Interfaces;
using Application.Models;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly SearchIndexClient _indexClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SearchService> _logger;
    private readonly ITeacherService _teacherService;
    private readonly ISchoolService _schoolService;
    private readonly INoticeService _noticeService;

    private const string TeachersIndexName = "teachers-index";
    private const string SchoolsIndexName = "schools-index";
    private const string NoticesIndexName = "notices-index";

    public SearchService(
        IConfiguration configuration,
        ILogger<SearchService> logger,
        ITeacherService teacherService,
        ISchoolService schoolService,
        INoticeService noticeService)
    {
        _configuration = configuration;
        _logger = logger;
        _teacherService = teacherService;
        _schoolService = schoolService;
        _noticeService = noticeService;

        var searchServiceEndpoint = _configuration["AzureSearch:ServiceEndpoint"];
        var searchServiceApiKey = _configuration["AzureSearch:ApiKey"];

        if (string.IsNullOrEmpty(searchServiceEndpoint) || string.IsNullOrEmpty(searchServiceApiKey))
        {
            throw new InvalidOperationException("Azure Search configuration is missing. Please configure AzureSearch:ServiceEndpoint and AzureSearch:ApiKey.");
        }

        _indexClient = new SearchIndexClient(new Uri(searchServiceEndpoint), new AzureKeyCredential(searchServiceApiKey));
    }

    public async Task InitializeIndexesAsync()
    {
        try
        {
            _logger.LogInformation("Initializing Azure Cognitive Search indexes...");

            // Create indexes if they don't exist
            await CreateIndexIfNotExistsAsync<TeacherSearchDocument>(TeachersIndexName);
            await CreateIndexIfNotExistsAsync<SchoolSearchDocument>(SchoolsIndexName);
            await CreateIndexIfNotExistsAsync<NoticeSearchDocument>(NoticesIndexName);

            _logger.LogInformation("Azure Cognitive Search indexes initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Azure Cognitive Search indexes.");
            throw;
        }
    }

    public async Task<bool> IndexExistsAsync(string indexName)
    {
        try
        {
            await _indexClient.GetIndexAsync(indexName);
            return true;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return false;
        }
    }

    public async Task CreateIndexAsync<T>(string indexName) where T : class
    {
        var definition = new SearchIndex(indexName)
        {
            Fields = new FieldBuilder().Build(typeof(T))
        };

        // Add suggesters for autocomplete
        definition.Suggesters.Add(new SearchSuggester("sg", "Name", "Title"));

        await _indexClient.CreateIndexAsync(definition);
        _logger.LogInformation($"Created search index: {indexName}");
    }

    private async Task CreateIndexIfNotExistsAsync<T>(string indexName) where T : class
    {
        if (!await IndexExistsAsync(indexName))
        {
            await CreateIndexAsync<T>(indexName);
        }
    }

    public async Task DeleteIndexAsync(string indexName)
    {
        await _indexClient.DeleteIndexAsync(indexName);
        _logger.LogInformation($"Deleted search index: {indexName}");
    }

    public async Task IndexDocumentAsync<T>(string indexName, T document) where T : class
    {
        var searchClient = _indexClient.GetSearchClient(indexName);
        await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(new[] { document }));
    }

    public async Task IndexDocumentsAsync<T>(string indexName, IEnumerable<T> documents) where T : class
    {
        var searchClient = _indexClient.GetSearchClient(indexName);
        var batch = IndexDocumentsBatch.Upload(documents);
        await searchClient.IndexDocumentsAsync(batch);
    }

    public async Task DeleteDocumentAsync(string indexName, string key)
    {
        var searchClient = _indexClient.GetSearchClient(indexName);
        await searchClient.DeleteDocumentsAsync("Id", new[] { key });
    }

    public async Task<SearchResponse<TeacherSearchDocument>> SearchTeachersAsync(SearchRequest request)
    {
        var searchClient = _indexClient.GetSearchClient(TeachersIndexName);
        
        var options = new SearchOptions
        {
            Size = request.Top,
            Skip = request.Skip,
            IncludeTotalCount = request.IncludeTotalCount
        };

        if (request.Filters != null)
        {
            foreach (var filter in request.Filters)
            {
                options.Filter = string.IsNullOrEmpty(options.Filter) ? filter : $"{options.Filter} and {filter}";
            }
        }

        if (request.Facets != null)
        {
            foreach (var facet in request.Facets)
            {
                options.Facets.Add(facet);
            }
        }

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            options.OrderBy.Add(request.OrderBy);
        }

        var response = await searchClient.SearchAsync<TeacherSearchDocument>(request.Query, options);
        
        return new SearchResponse<TeacherSearchDocument>
        {
            Results = response.Value.GetResults().Select(r => r.Document).ToList(),
            TotalCount = response.Value.TotalCount,
            Facets = response.Value.Facets?.ToDictionary(
                f => f.Key, 
                f => f.Value.Select(v => new Application.Models.FacetResult { Value = v.Value, Count = v.Count }).ToList() as IList<Application.Models.FacetResult>
            )
        };
    }

    public async Task<SearchResponse<SchoolSearchDocument>> SearchSchoolsAsync(SearchRequest request)
    {
        var searchClient = _indexClient.GetSearchClient(SchoolsIndexName);
        
        var options = new SearchOptions
        {
            Size = request.Top,
            Skip = request.Skip,
            IncludeTotalCount = request.IncludeTotalCount
        };

        if (request.Filters != null)
        {
            foreach (var filter in request.Filters)
            {
                options.Filter = string.IsNullOrEmpty(options.Filter) ? filter : $"{options.Filter} and {filter}";
            }
        }

        if (request.Facets != null)
        {
            foreach (var facet in request.Facets)
            {
                options.Facets.Add(facet);
            }
        }

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            options.OrderBy.Add(request.OrderBy);
        }

        var response = await searchClient.SearchAsync<SchoolSearchDocument>(request.Query, options);
        
        return new SearchResponse<SchoolSearchDocument>
        {
            Results = response.Value.GetResults().Select(r => r.Document).ToList(),
            TotalCount = response.Value.TotalCount,
            Facets = response.Value.Facets?.ToDictionary(
                f => f.Key, 
                f => f.Value.Select(v => new Application.Models.FacetResult { Value = v.Value, Count = v.Count }).ToList() as IList<Application.Models.FacetResult>
            )
        };
    }

    public async Task<SearchResponse<NoticeSearchDocument>> SearchNoticesAsync(SearchRequest request)
    {
        var searchClient = _indexClient.GetSearchClient(NoticesIndexName);
        
        var options = new SearchOptions
        {
            Size = request.Top,
            Skip = request.Skip,
            IncludeTotalCount = request.IncludeTotalCount
        };

        if (request.Filters != null)
        {
            foreach (var filter in request.Filters)
            {
                options.Filter = string.IsNullOrEmpty(options.Filter) ? filter : $"{options.Filter} and {filter}";
            }
        }

        if (request.Facets != null)
        {
            foreach (var facet in request.Facets)
            {
                options.Facets.Add(facet);
            }
        }

        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            options.OrderBy.Add(request.OrderBy);
        }

        var response = await searchClient.SearchAsync<NoticeSearchDocument>(request.Query, options);
        
        return new SearchResponse<NoticeSearchDocument>
        {
            Results = response.Value.GetResults().Select(r => r.Document).ToList(),
            TotalCount = response.Value.TotalCount,
            Facets = response.Value.Facets?.ToDictionary(
                f => f.Key, 
                f => f.Value.Select(v => new Application.Models.FacetResult { Value = v.Value, Count = v.Count }).ToList() as IList<Application.Models.FacetResult>
            )
        };
    }

    public async Task<UnifiedSearchResponse> UnifiedSearchAsync(UnifiedSearchRequest request)
    {
        var results = new List<UnifiedSearchResult>();
        var typeCounts = new Dictionary<string, long>();

        var searchTypes = request.SearchTypes ?? new[] { "teachers", "schools", "notices" };

        foreach (var searchType in searchTypes)
        {
            switch (searchType.ToLower())
            {
                case "teachers":
                    var teacherResults = await SearchTeachersAsync(new SearchRequest
                    {
                        Query = request.Query,
                        Filters = request.Filters,
                        Skip = request.Skip,
                        Top = request.Top
                    });
                    
                    results.AddRange(teacherResults.Results.Select(t => new UnifiedSearchResult
                    {
                        Type = "teacher",
                        Id = t.Id,
                        Title = t.Name,
                        Description = $"{t.SchoolName}, {t.District}",
                        Category = t.Class,
                        Date = t.CreatedDate,
                        Data = t
                    }));
                    
                    typeCounts["teachers"] = teacherResults.TotalCount ?? 0;
                    break;

                case "schools":
                    var schoolResults = await SearchSchoolsAsync(new SearchRequest
                    {
                        Query = request.Query,
                        Filters = request.Filters,
                        Skip = request.Skip,
                        Top = request.Top
                    });
                    
                    results.AddRange(schoolResults.Results.Select(s => new UnifiedSearchResult
                    {
                        Type = "school",
                        Id = s.Id,
                        Title = s.Name,
                        Description = $"{s.Address}, {s.District}",
                        Category = s.Type,
                        Date = s.EstablishedDate,
                        Data = s
                    }));
                    
                    typeCounts["schools"] = schoolResults.TotalCount ?? 0;
                    break;

                case "notices":
                    var noticeResults = await SearchNoticesAsync(new SearchRequest
                    {
                        Query = request.Query,
                        Filters = request.Filters,
                        Skip = request.Skip,
                        Top = request.Top
                    });
                    
                    results.AddRange(noticeResults.Results.Select(n => new UnifiedSearchResult
                    {
                        Type = "notice",
                        Id = n.Id,
                        Title = n.Title,
                        Description = n.Message.Length > 100 ? n.Message.Substring(0, 100) + "..." : n.Message,
                        Category = n.Category,
                        Date = n.PostedDate,
                        Data = n
                    }));
                    
                    typeCounts["notices"] = noticeResults.TotalCount ?? 0;
                    break;
            }
        }

        return new UnifiedSearchResponse
        {
            Results = results.OrderByDescending(r => r.Date).Take(request.Top).ToList(),
            TotalCount = results.Count,
            TypeCounts = typeCounts
        };
    }

    public async Task SyncAllDataAsync()
    {
        _logger.LogInformation("Starting full data synchronization with Azure Cognitive Search...");
        
        await SyncTeachersAsync();
        await SyncSchoolsAsync();
        await SyncNoticesAsync();
        
        _logger.LogInformation("Full data synchronization completed.");
    }

    public async Task SyncTeachersAsync()
    {
        try
        {
            _logger.LogInformation("Syncing teachers data...");
            
            var teachers = await _teacherService.GetAllTeachersAsync();
            var searchDocuments = teachers.Select(t => new TeacherSearchDocument
            {
                Id = t.Id.ToString(),
                Name = t.TeacherName,
                Email = t.Email,
                Address = t.Address,
                District = t.District,
                Pincode = t.Pincode,
                SchoolName = t.SchoolName,
                Class = t.ClassTeaching,
                CreatedDate = t.DateOfJoining,
                SchoolId = t.SchoolId
            });

            await IndexDocumentsAsync(TeachersIndexName, searchDocuments);
            _logger.LogInformation($"Synced {searchDocuments.Count()} teachers to search index.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync teachers data.");
            throw;
        }
    }

    public async Task SyncSchoolsAsync()
    {
        try
        {
            _logger.LogInformation("Syncing schools data...");
            
            var schools = await _schoolService.GetAllSchoolsAsync();
            var searchDocuments = schools.Select(s => new SchoolSearchDocument
            {
                Id = s.Id.ToString(),
                Name = s.SchoolName,
                District = s.District,
                Address = $"{s.Village}, {s.Block}",
                Pincode = "", // Not available in SchoolDto
                Type = s.SchoolType,
                IsActive = s.IsActive,
                EstablishedDate = s.EstablishedDate,
                TeacherCount = s.TotalTeachers
            });

            await IndexDocumentsAsync(SchoolsIndexName, searchDocuments);
            _logger.LogInformation($"Synced {searchDocuments.Count()} schools to search index.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync schools data.");
            throw;
        }
    }

    public async Task SyncNoticesAsync()
    {
        try
        {
            _logger.LogInformation("Syncing notices data...");
            
            var notices = await _noticeService.GetAllActiveNoticesAsync(0); // Get all notices
            var searchDocuments = notices.Select(n => new NoticeSearchDocument
            {
                Id = n.Id.ToString(),
                Title = n.Title,
                Message = n.Message,
                Category = n.Category,
                Priority = n.Priority,
                PostedByUserName = n.PostedByUserName,
                PostedDate = n.PostedDate,
                IsActive = n.IsActive,
                ReplyCount = n.ReplyCount
            });

            await IndexDocumentsAsync(NoticesIndexName, searchDocuments);
            _logger.LogInformation($"Synced {searchDocuments.Count()} notices to search index.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync notices data.");
            throw;
        }
    }

    public async Task<IList<string>> GetSuggestionsAsync(string indexName, string query, string suggesterName)
    {
        var searchClient = _indexClient.GetSearchClient(indexName);
        var options = new SuggestOptions
        {
            Size = 10
        };

        var response = await searchClient.SuggestAsync<dynamic>(query, suggesterName, options);
        return response.Value.Results.Select(r => r.Text).ToList();
    }

    public async Task<IList<string>> AutocompleteAsync(string indexName, string query, string suggesterName)
    {
        var searchClient = _indexClient.GetSearchClient(indexName);
        var options = new AutocompleteOptions
        {
            Size = 10,
            Mode = AutocompleteMode.TwoTerms
        };

        var response = await searchClient.AutocompleteAsync(query, suggesterName, options);
        return response.Value.Results.Select(r => r.Text).ToList();
    }
}