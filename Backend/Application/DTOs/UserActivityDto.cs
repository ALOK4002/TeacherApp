using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class UserActivityDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("activityType")]
    public ActivityType ActivityType { get; set; }
    
    [JsonPropertyName("activityDescription")]
    public string ActivityDescription { get; set; } = string.Empty;
    
    [JsonPropertyName("entityType")]
    public string? EntityType { get; set; }
    
    [JsonPropertyName("entityId")]
    public int? EntityId { get; set; }
    
    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }
    
    [JsonPropertyName("activityDate")]
    public DateTime ActivityDate { get; set; }
    
    [JsonPropertyName("activityIcon")]
    public string ActivityIcon { get; set; } = string.Empty;
    
    [JsonPropertyName("activityColor")]
    public string ActivityColor { get; set; } = string.Empty;
}

public class CreateUserActivityDto
{
    [JsonPropertyName("activityType")]
    public ActivityType ActivityType { get; set; }
    
    [JsonPropertyName("activityDescription")]
    public string ActivityDescription { get; set; } = string.Empty;
    
    [JsonPropertyName("entityType")]
    public string? EntityType { get; set; }
    
    [JsonPropertyName("entityId")]
    public int? EntityId { get; set; }
    
    [JsonPropertyName("metadata")]
    public string? Metadata { get; set; }
}