using Application.Models;

namespace Application.Interfaces;

public interface ISearchService
{
    // Index Management
    Task InitializeIndexesAsync();
    Task<bool> IndexExistsAsync(string indexName);
    Task CreateIndexAsync<T>(string indexName) where T : class;
    Task DeleteIndexAsync(string indexName);

    // Document Management
    Task IndexDocumentAsync<T>(string indexName, T document) where T : class;
    Task IndexDocumentsAsync<T>(string indexName, IEnumerable<T> documents) where T : class;
    Task DeleteDocumentAsync(string indexName, string key);

    // Search Operations
    Task<SearchResponse<TeacherSearchDocument>> SearchTeachersAsync(SearchRequest request);
    Task<SearchResponse<SchoolSearchDocument>> SearchSchoolsAsync(SearchRequest request);
    Task<SearchResponse<NoticeSearchDocument>> SearchNoticesAsync(SearchRequest request);
    Task<UnifiedSearchResponse> UnifiedSearchAsync(UnifiedSearchRequest request);

    // Data Synchronization
    Task SyncAllDataAsync();
    Task SyncTeachersAsync();
    Task SyncSchoolsAsync();
    Task SyncNoticesAsync();

    // Suggestions and Autocomplete
    Task<IList<string>> GetSuggestionsAsync(string indexName, string query, string suggesterName);
    Task<IList<string>> AutocompleteAsync(string indexName, string query, string suggesterName);
}