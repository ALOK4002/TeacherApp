using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ITeacherDocumentRepository _documentRepository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, ITeacherDocumentRepository documentRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _documentRepository = documentRepository;
    }

    public async Task<SubscriptionDto?> GetUserSubscriptionAsync(int userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        return subscription != null ? MapToDto(subscription) : null;
    }

    public async Task<SubscriptionDto> CreateFreeSubscriptionAsync(int userId)
    {
        var subscription = new Subscription
        {
            UserId = userId,
            Tier = SubscriptionTier.Free,
            Status = SubscriptionStatus.Active,
            StartDate = DateTime.UtcNow,
            DocumentUploadLimit = 3,
            FileSizeLimitInBytes = 512000, // 500KB
            DocumentsUploaded = 0,
            IsActive = true
        };

        var created = await _subscriptionRepository.AddAsync(subscription);
        return MapToDto(created);
    }

    public async Task<SubscriptionDto> UpgradeToPremiuAsync(int userId, int paymentId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        if (subscription == null)
        {
            throw new InvalidOperationException("User subscription not found");
        }

        subscription.Tier = SubscriptionTier.Premium;
        subscription.Status = SubscriptionStatus.Active;
        subscription.DocumentUploadLimit = 10;
        subscription.FileSizeLimitInBytes = 1048576; // 1MB
        subscription.EndDate = DateTime.UtcNow.AddYears(1); // 1 year premium

        var updated = await _subscriptionRepository.UpdateAsync(subscription);
        return MapToDto(updated);
    }

    public async Task<bool> CanUploadDocumentAsync(int userId, long fileSizeInBytes)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        if (subscription == null)
        {
            return false;
        }

        // Check file size limit
        if (fileSizeInBytes > subscription.FileSizeLimitInBytes)
        {
            return false;
        }

        // Check upload count limit
        if (subscription.DocumentsUploaded >= subscription.DocumentUploadLimit)
        {
            return false;
        }

        return true;
    }

    public async Task IncrementDocumentCountAsync(int userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        if (subscription != null)
        {
            subscription.DocumentsUploaded++;
            await _subscriptionRepository.UpdateAsync(subscription);
        }
    }

    public async Task<bool> HasReachedUploadLimitAsync(int userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);
        return subscription != null && subscription.DocumentsUploaded >= subscription.DocumentUploadLimit;
    }

    private static SubscriptionDto MapToDto(Subscription subscription)
    {
        return new SubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            Tier = subscription.Tier,
            Status = subscription.Status,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            DocumentUploadLimit = subscription.DocumentUploadLimit,
            FileSizeLimitInBytes = subscription.FileSizeLimitInBytes,
            FileSizeLimitFormatted = FormatFileSize(subscription.FileSizeLimitInBytes),
            DocumentsUploaded = subscription.DocumentsUploaded,
            RemainingUploads = Math.Max(0, subscription.DocumentUploadLimit - subscription.DocumentsUploaded),
            IsActive = subscription.IsActive,
            CreatedDate = subscription.CreatedDate
        };
    }

    private static string FormatFileSize(long bytes)
    {
        if (bytes >= 1048576)
            return $"{bytes / 1048576.0:F1} MB";
        if (bytes >= 1024)
            return $"{bytes / 1024.0:F1} KB";
        return $"{bytes} bytes";
    }
}