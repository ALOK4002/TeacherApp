using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserActivityService
{
    Task<UserActivityDto> LogActivityAsync(int userId, CreateUserActivityDto dto);
    Task<IEnumerable<UserActivityDto>> GetUserActivitiesAsync(int userId, int page = 1, int pageSize = 20);
    Task LogDocumentUploadAsync(int userId, int documentId, string fileName);
    Task LogDocumentDeleteAsync(int userId, int documentId, string fileName);
    Task LogPaymentInitiatedAsync(int userId, int paymentId, decimal amount);
    Task LogPaymentCompletedAsync(int userId, int paymentId, decimal amount);
    Task LogSubscriptionUpgradeAsync(int userId, SubscriptionTier fromTier, SubscriptionTier toTier);
}