using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Text.Json;

namespace Infrastructure.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IUserActivityRepository _activityRepository;

    public UserActivityService(IUserActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<UserActivityDto> LogActivityAsync(int userId, CreateUserActivityDto dto)
    {
        var activity = new UserActivity
        {
            UserId = userId,
            ActivityType = dto.ActivityType,
            ActivityDescription = dto.ActivityDescription,
            EntityType = dto.EntityType,
            EntityId = dto.EntityId,
            Metadata = dto.Metadata,
            IsActive = true
        };

        var created = await _activityRepository.AddAsync(activity);
        return MapToDto(created);
    }

    public async Task<IEnumerable<UserActivityDto>> GetUserActivitiesAsync(int userId, int page = 1, int pageSize = 20)
    {
        var activities = await _activityRepository.GetByUserIdAsync(userId, page, pageSize);
        return activities.Select(MapToDto);
    }

    public async Task LogDocumentUploadAsync(int userId, int documentId, string fileName)
    {
        var dto = new CreateUserActivityDto
        {
            ActivityType = ActivityType.DocumentUploaded,
            ActivityDescription = $"Uploaded document: {fileName}",
            EntityType = "Document",
            EntityId = documentId,
            Metadata = JsonSerializer.Serialize(new { FileName = fileName })
        };

        await LogActivityAsync(userId, dto);
    }

    public async Task LogDocumentDeleteAsync(int userId, int documentId, string fileName)
    {
        var dto = new CreateUserActivityDto
        {
            ActivityType = ActivityType.DocumentDeleted,
            ActivityDescription = $"Deleted document: {fileName}",
            EntityType = "Document",
            EntityId = documentId,
            Metadata = JsonSerializer.Serialize(new { FileName = fileName })
        };

        await LogActivityAsync(userId, dto);
    }

    public async Task LogPaymentInitiatedAsync(int userId, int paymentId, decimal amount)
    {
        var dto = new CreateUserActivityDto
        {
            ActivityType = ActivityType.PaymentInitiated,
            ActivityDescription = $"Initiated payment of ₹{amount:F2} for premium subscription",
            EntityType = "Payment",
            EntityId = paymentId,
            Metadata = JsonSerializer.Serialize(new { Amount = amount, Currency = "INR" })
        };

        await LogActivityAsync(userId, dto);
    }

    public async Task LogPaymentCompletedAsync(int userId, int paymentId, decimal amount)
    {
        var dto = new CreateUserActivityDto
        {
            ActivityType = ActivityType.PaymentCompleted,
            ActivityDescription = $"Payment of ₹{amount:F2} completed successfully",
            EntityType = "Payment",
            EntityId = paymentId,
            Metadata = JsonSerializer.Serialize(new { Amount = amount, Currency = "INR" })
        };

        await LogActivityAsync(userId, dto);
    }

    public async Task LogSubscriptionUpgradeAsync(int userId, SubscriptionTier fromTier, SubscriptionTier toTier)
    {
        var dto = new CreateUserActivityDto
        {
            ActivityType = ActivityType.SubscriptionUpgraded,
            ActivityDescription = $"Subscription upgraded from {fromTier} to {toTier}",
            EntityType = "Subscription",
            EntityId = null,
            Metadata = JsonSerializer.Serialize(new { FromTier = fromTier.ToString(), ToTier = toTier.ToString() })
        };

        await LogActivityAsync(userId, dto);
    }

    private static UserActivityDto MapToDto(UserActivity activity)
    {
        return new UserActivityDto
        {
            Id = activity.Id,
            ActivityType = activity.ActivityType,
            ActivityDescription = activity.ActivityDescription,
            EntityType = activity.EntityType,
            EntityId = activity.EntityId,
            Metadata = activity.Metadata,
            ActivityDate = activity.ActivityDate,
            ActivityIcon = GetActivityIcon(activity.ActivityType),
            ActivityColor = GetActivityColor(activity.ActivityType)
        };
    }

    private static string GetActivityIcon(ActivityType activityType)
    {
        return activityType switch
        {
            ActivityType.DocumentUploaded => "📤",
            ActivityType.DocumentDeleted => "🗑️",
            ActivityType.DocumentViewed => "👁️",
            ActivityType.DocumentDownloaded => "⬇️",
            ActivityType.DocumentEmailed => "📧",
            ActivityType.PaymentInitiated => "💳",
            ActivityType.PaymentCompleted => "✅",
            ActivityType.PaymentFailed => "❌",
            ActivityType.SubscriptionUpgraded => "⬆️",
            ActivityType.SubscriptionExpired => "⏰",
            ActivityType.ProfileCreated => "👤",
            ActivityType.ProfileUpdated => "✏️",
            ActivityType.Login => "🔑",
            ActivityType.Logout => "🚪",
            _ => "📋"
        };
    }

    private static string GetActivityColor(ActivityType activityType)
    {
        return activityType switch
        {
            ActivityType.DocumentUploaded => "#107c10",
            ActivityType.DocumentDeleted => "#d13438",
            ActivityType.DocumentViewed => "#0078d4",
            ActivityType.DocumentDownloaded => "#8764b8",
            ActivityType.DocumentEmailed => "#ca5010",
            ActivityType.PaymentInitiated => "#ffaa44",
            ActivityType.PaymentCompleted => "#107c10",
            ActivityType.PaymentFailed => "#d13438",
            ActivityType.SubscriptionUpgraded => "#107c10",
            ActivityType.SubscriptionExpired => "#ffaa44",
            ActivityType.ProfileCreated => "#0078d4",
            ActivityType.ProfileUpdated => "#8764b8",
            ActivityType.Login => "#107c10",
            ActivityType.Logout => "#737373",
            _ => "#737373"
        };
    }
}