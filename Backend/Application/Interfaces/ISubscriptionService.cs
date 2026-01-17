using Application.DTOs;

namespace Application.Interfaces;

public interface ISubscriptionService
{
    Task<SubscriptionDto?> GetUserSubscriptionAsync(int userId);
    Task<SubscriptionDto> CreateFreeSubscriptionAsync(int userId);
    Task<SubscriptionDto> UpgradeToPremiuAsync(int userId, int paymentId);
    Task<bool> CanUploadDocumentAsync(int userId, long fileSizeInBytes);
    Task IncrementDocumentCountAsync(int userId);
    Task<bool> HasReachedUploadLimitAsync(int userId);
}